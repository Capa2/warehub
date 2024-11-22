using NLog;
using warehub;
using warehub.controller;
using warehub.db;
using warehub.utils;

class Program
{
    /// <summary>
    /// Entry point of the application, responsible for initializing logging, 
    /// establishing database connection, and coordinating core operations.
    /// </summary>
    static void Main(string[] args)
    {
        LoggerConfig.ConfigureLogging();
        var logger = LogManager.GetCurrentClassLogger();
        logger.Info("Application started.");
        logger.Info("Initializing Database...");
        DbConnection.Initialize();
        DbConnection.Connect();
        //logger.Info("Populating...");
        //ProductPopulater.Populate();
        DbConnection.Disconnect();
        LogManager.Shutdown();
    }
}
