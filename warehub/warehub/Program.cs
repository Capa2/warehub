using NLog;
using warehub;
using warehub.controller;
using warehub.db;
using warehub.utils;

class Program
{
    static void Main(string[] args)
    {
        LoggerConfig.ConfigureLogging();
        var logger = LogManager.GetCurrentClassLogger();
        logger.Info("Program: Application started.");

        ProductPopulater.Populate();
        
        LogManager.Shutdown();
    }
}
