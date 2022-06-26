using System;
using System.Data;
using System.Data.SQLite;
using Microsoft.Data.Sqlite;
namespace Testing.DbConnect
{
    public class SqliteConnect
    {
        private readonly string _connectionstring;

        public SqliteConnect(string connectionString)
        {
            _connectionstring = connectionString;
        }

        public IDbConnection Connect()
        {
            return new SqliteConnection(_connectionstring);
        }
    }
}