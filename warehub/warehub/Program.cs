using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using NLog;
using warehub;
using warehub.db;
using warehub.utils;

class Program
{
    static void Main(string[] args)
    {
        LoggerConfig.ConfigureLogging();
        var logger = LogManager.GetCurrentClassLogger();
        logger.Info("Application started.");
        
        logger.Info("Populating...");
        ProductPopulater.Populate();
        
        LogManager.Shutdown();
    }
}