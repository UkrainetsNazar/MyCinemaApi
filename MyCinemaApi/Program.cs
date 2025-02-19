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
using Cinema.Application.UseCases;
using Cinema.Application.UseCases.MovieUseCases;
using Cinema.Application.UseCases.HallUseCases;
using Cinema.Application.UseCases.SessionUseCases;
using Cinema.Application.UseCases.TicketUseCases;
using Cinema.Infrastructure.ExternalServices;


namespace MyCinemaApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });


        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/errors.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Services.AddDbContext<CinemaDbContext>(options =>
        {
            options.UseInMemoryDatabase("MyInMemoryDb");
        });

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "SampleInstance";
        });

        builder.Services.AddScoped<IMovieRepository, MovieRepository>();
        builder.Services.AddScoped<IHallRepository, HallRepository>();
        builder.Services.AddScoped<ISessionRepository, SessionRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ITicketRepository, TicketRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

        builder.Services.AddScoped<UseCaseManager>();

        builder.Services.AddScoped<AddMovieHandler>();
        builder.Services.AddScoped<DeleteMovieHandler>();
        builder.Services.AddScoped<UpdateMovieHandler>();
        builder.Services.AddScoped<CreateHallHandler>();
        builder.Services.AddScoped<AddSessionHandler>();
        builder.Services.AddScoped<GetSessionDetailsHandler>();
        builder.Services.AddScoped<GetAllSessionsHandler>();
        builder.Services.AddScoped<GetMoviesByDateHandler>();
        builder.Services.AddScoped<GetMovieWithSessionsHandler>();
        builder.Services.AddScoped<MovieRatingHandler>();
        builder.Services.AddScoped<BuyTicketHandler>();
        builder.Services.AddScoped<GetUserTicketsHandler>();


        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateMovieValidator>();

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Host.UseSerilog();

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