using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseInMemoryDatabase("DefaultConnection"));

builder.Services.AddIdentity<User, IdentityRole>(
    options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<CinemaDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = 
    options.DefaultChallengeScheme = 
    options.DefaultSignInScheme = 
    options.DefaultSignOutScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
