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
using Cinema.Presentation.Middleware;

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
            .AddEntityFrameworkStores<CinemaDbContext>();

        // Add Authentication + JWT
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options => {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                IssuerSigningKey = secret,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });


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
        builder.Services.AddSwaggerGen(swagger =>
        {
            //This is to generate the Default UI of Swagger Documentation    
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ASP.NET Core Web API"
            });
            // To Enable authorization using Swagger (JWT)    
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token.",
            });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                    new string[] {}
                }
            });
        });

        // Authorization and CORS
        builder.Services.AddCors(options => {
            options.AddPolicy("MyPolicy",
                              policy => policy.AllowAnyMethod()
                              .AllowAnyOrigin()
                              .AllowAnyHeader());
        });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowAll");
        app.UseHttpsRedirection();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                RoleInitializer.Initialize(roleManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database roles.");
            }
        }

        // Middleware
        app.UseMiddleware<UnauthorizedMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();

        // Authentication and Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSerilogRequestLogging();
        app.MapControllers();

        app.Run();
    }
}

public static class RoleInitializer
{
    public static void Initialize(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
            }
        }
    }
}
