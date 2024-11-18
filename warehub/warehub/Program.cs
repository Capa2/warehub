using NLog;
using warehub;
<<<<<<< HEAD
=======
using warehub.controller;
using warehub.db;
>>>>>>> fd6900df05e0f2fe11ae2489f01dd35b66e06276
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