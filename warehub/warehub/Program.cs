using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using warehub;
using warehub.controller;
using warehub.db;
using warehub.repository;
using warehub.services;
using warehub.services.interfaces;
using warehub.utils;
using Config = warehub.Config;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddFastEndpoints(); // Register FastEndpoints

builder.Services.AddSingleton<IDbConnection>(provider =>
{
    var config = Config.GetInstance();
    var connectionString = config.GetConnectionString("localhost");
    return new DbConnection(connectionString);
});
builder.Services.AddScoped<IProductRepository, ProductRepository>(); 
builder.Services.AddScoped<IProductService, ProductService>();    // Scoped for request lifecycle
var app = builder.Build();

// Configure middleware
//app.UseAuthorization();
app.UseFastEndpoints(); // Enable FastEndpoints

app.Run();
