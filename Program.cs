global using Microsoft.EntityFrameworkCore;
global using dotnet_authentication.Models;
global using dotnet_authentication.Data;
global using System.ComponentModel.DataAnnotations;
global using System.Net;
global using dotnet_authentication.utils;
global using dotnet_authentication.Repository;
global using System.Security.Claims;
global using Microsoft.AspNetCore.Mvc;
global using dotnet_authentication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<AuthRepo, AuthService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
