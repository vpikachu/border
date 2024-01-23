using System.Data;
using Microsoft.Data.Sqlite;

namespace border.api.Services
{
    public class SqliteDatabaseHelper : IDatabaseHelper
    {
        private readonly string _connectionString;
        public SqliteDatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IDbConnection CreateConnection() => new SqliteConnection(_connectionString);

        public void CreateDatabase()
        {
            var builder = new SqliteConnectionStringBuilder(_connectionString)
            {
                Mode = SqliteOpenMode.ReadWriteCreate
            };
            if (!System.IO.File.Exists(builder.DataSource))
            {
                using (IDbConnection connection = new SqliteConnection(builder.ToString()))
                {
                    connection.Open();
                    connection.Close();
                }
            }
        }
    }
}