using System;
using Dapper;
using System.Data;
using System.Data.SQLite;
using Microsoft.Data.Sqlite;
using Testing.DbConnect;

namespace Testing.BuildDb
{


    public class SQliteDbInitializer
    {
        private const string CREATE_PLACED_BETS = @"CREATE TABLE IF NOT EXISTS PLACED_BETS
         (
             Id TEXT PRIMARY KEY, 
             Chosennumber INTEGER, 
             Colour TEXT, 
             EONumber INTEGER,
             BetID INTEGER,
             Expired TEXT,
             PayoutAmount FLOAT
        )";

        private const string CREATE_SPINNED_BETS = @"CREATE TABLE IF NOT EXISTS SPINNED_BETS
         (
             BetId TEXT PRIMARY KEY, 
             Chosennumber INTEGER, 
             Colour TEXT, 
             EONumber INTEGER
        )";

        

        private readonly SqliteConnect _sqliteDbConnect;
        public SQliteDbInitializer(SqliteConnect sqliteonnect)
        {
            _sqliteDbConnect = sqliteonnect;
        }
        public void Initialize()
        {
            using (IDbConnection database = _sqliteDbConnect.Connect())
            {
                database.Execute(CREATE_PLACED_BETS);
                database.Execute(CREATE_SPINNED_BETS);
            }
        }

    }
}