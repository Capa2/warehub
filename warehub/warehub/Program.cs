using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using warehub;
using warehub.db;

class Program
{
    static void Main(string[] args)
    {
        ProductPopulater.Populate();
    }
}