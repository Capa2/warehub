using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using warehub;
using warehub.controller;
using warehub.db;
using warehub.repository;
using warehub.services;
using warehub.utils;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddFastEndpoints(); // Register FastEndpoints


builder.Services.AddScoped<IProductRepository, ProductRepository>(); 
builder.Services.AddScoped<ProductService>();    // Scoped for request lifecycle
var app = builder.Build();

// Configure middleware
//app.UseAuthorization();
app.UseFastEndpoints(); // Enable FastEndpoints

app.Run();
//class Program
//{
//
//    static void Main(string[] args)
//    {
//
//        
//        LoggerConfig.ConfigureLogging();
//        var logger = LogManager.GetCurrentClassLogger();
//        logger.Info("Application started.");
//
//        logger.Info("Populating...");
//        ProductPopulater.Populate();
//        
//        LogManager.Shutdown();
//    }
//}
