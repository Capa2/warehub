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
        logger.Info("Application started.");
        logger.Info("Initializing Database...");
        DbConnection.Initialize();
        DbConnection.Instance.Connect();
        //logger.Info("Populating...");
        //ProductPopulater.Populate();
        DbConnection.Disconnect();
        LogManager.Shutdown();
    }
}
