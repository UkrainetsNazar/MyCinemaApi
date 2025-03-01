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
using Cinema.Infrastructure.Caching;
using Cinema.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Cinema.Domain.ValueObjects;
using System.Globalization;
using Cinema.Application.UseCases.AuthServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace MyCinemaApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // JWT configuration
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
        var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
        if (string.IsNullOrEmpty(jwtOptions?.Secret))
        {
            throw new ArgumentNullException("JwtOptions:Key", "JWT Secret key is missing in configuration.");
        }
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
        var issuer = jwtOptions.Issuer;
        var audience = jwtOptions.Audience;

        // Localization
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

        // Setting up DB
        var useInMemoryDB = builder.Configuration.GetValue<bool>("UseInMemoryDB");

        if (useInMemoryDB)
        {
            builder.Services.AddDbContext<CinemaDbContext>(options =>
                options.UseInMemoryDatabase("AuthDb"));
        }
        else
        {
            builder.Services.AddDbContext<CinemaDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure()));
        }

        // Identity
        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
        })
            .AddEntityFrameworkStores<CinemaDbContext>()
            .AddDefaultTokenProviders();

        // Add Authentication + JWT
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = secret,
                    RoleClaimType = ClaimTypes.Role
                };
            });

        builder.Services.AddAuthorization();

        // Logging
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/errors.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();


        // Controllers
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

        // Caching
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<ICacheService, InMemoryCacheService>();

        // External services
        builder.Services.Configure<TmdbSettings>(builder.Configuration.GetSection("TMDB"));
        builder.Services.AddHttpClient<TmdbService>();
        builder.Services.AddScoped<TmdbService>();

        // Scopes
        builder.Services.AddScoped<IMovieRepository, MovieRepository>();
        builder.Services.AddScoped<IHallRepository, HallRepository>();
        builder.Services.AddScoped<ISessionRepository, SessionRepository>();
        builder.Services.AddScoped<ITicketRepository, TicketRepository>();
        builder.Services.AddScoped<ISeatRepository, SeatRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<UseCaseManager>();
        builder.Services.AddScoped<UserManager<User>>();
        builder.Services.AddScoped<RoleManager<IdentityRole>>();

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

        // FluentValidation
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateMovieValidator>();

        // AutoMapper
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        // Serilog
        builder.Host.UseSerilog();

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Cinema API", Version = "v1" });

            options.AddSecurityDefinition("CookieAuth", new OpenApiSecurityScheme
            {
                Name = "Token",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Cookie,
                Description = "Авторизація через JWT, який зберігається в cookie",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "CookieAuth"
                }
            },
            new string[] {}
        }
    });
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowAll");
        app.UseHttpsRedirection();

        // Authentication and Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Middleware
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSerilogRequestLogging();
        app.MapControllers();

        app.Run();
    }
}
