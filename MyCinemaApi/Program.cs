using Cinema.API.Middleware;
using Cinema.Application.Mapping;
using Serilog;
using FluentValidation.AspNetCore;
using FluentValidation;
using Cinema.Application.Validators;
using Cinema.Application.Interfaces;
using Cinema.Infrastructure.Repositories;
using Cinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCinemaApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information().WriteTo
                 .Console().WriteTo
                 .File("logs/errors.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

        builder.Services.AddDbContext<CinemaDbContext>(options =>
        {
            options.UseInMemoryDatabase("MyInMemoryDb");
        });

        builder.Services.AddScoped<IMovieRepository, MovieRepository>();
        builder.Services.AddScoped<IHallRepository, HallRepository>();
        builder.Services.AddScoped<ISessionRepository, SessionRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ITicketRepository, TicketRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Host.UseSerilog();

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateMovieValidator>();

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSerilogRequestLogging();
        app.MapControllers();

        app.Run();
    }
}