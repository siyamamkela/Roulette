using System;
using System.Data.SQLite;
using System.IO;
namespace Testing
{
    public class Database
    {
        public SQLiteConnection conn;
        public Database()
        {
            conn = new SQLiteConnection("Data Source = database.Roulette");

            if (!File.Exists("./database.Roulette")) {
                Console.WriteLine("Database Created");
                SQLiteConnection.CreateFile("database.Roulette");
            }
        }


    }
}