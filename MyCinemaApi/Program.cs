using Cinema.API.Middleware;
using Cinema.Application.Mapping;
using Serilog;

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

        builder.Host.UseSerilog();

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