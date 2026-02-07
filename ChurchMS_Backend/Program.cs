using ChurchMS_Backend.Data;
using ChurchMS_Backend.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<UserDbContext>(
    // I am using mysql pomelo package, so I need to use UseMySql method
    options => options.UseMySql(
        builder.Configuration.GetConnectionString("UserDatabase"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("UserDatabase"))
    ));


builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
