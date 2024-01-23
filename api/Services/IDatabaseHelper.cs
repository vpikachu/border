using System.Data;
using Microsoft.Data.Sqlite;


namespace border.api.Services
{
    public interface IDatabaseHelper
    {

        public IDbConnection CreateConnection();

        public void CreateDatabase();
    }
}
