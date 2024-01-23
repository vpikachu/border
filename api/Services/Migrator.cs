using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using border.api.Services;

namespace api.Services
{
    public static partial class Migrator
    {
        public static IApplicationBuilder DoMigrate(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var _databaseHelper = scope.ServiceProvider.GetService<IDatabaseHelper>();
                if (_databaseHelper == null) return app;
                _databaseHelper.CreateDatabase();
                using (IDbConnection connection = _databaseHelper.CreateConnection())
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT max(id) FROM sys_migrations;";
                    command.CommandType = CommandType.Text;
                    long? lastId = null;
                    try
                    {
                        lastId = (long?)command.ExecuteScalar();
                    }
                    catch { }
                    if (lastId == null) lastId = 0;
                    string[] files = System.IO.Directory.GetFiles("./database/migrations");
                    int[] ids = files.Select((f) => int.Parse(Path.GetFileName(f))).Order().ToArray();

                    if (ids.Length == 0) return app;

                    var updateCommand = connection.CreateCommand();
                    updateCommand.CommandType = CommandType.Text;
                    updateCommand.CommandText = "INSERT INTO sys_migrations (id, exec_time) values ($id, $exec_time)";

                    var paramId = updateCommand.CreateParameter();
                    paramId.ParameterName = "$id";
                    updateCommand.Parameters.Add(paramId);

                    var paramExecTime = updateCommand.CreateParameter();
                    paramExecTime.ParameterName = "$exec_time";
                    updateCommand.Parameters.Add(paramExecTime);

                    foreach (var id in ids)
                    {
                        if (id > lastId)
                        {
                            string script = System.IO.File.ReadAllText("./database/migrations/" + id.ToString());
                            command.CommandText = script;
                            command.ExecuteNonQuery();
                            paramId.Value = id;
                            paramExecTime.Value = DateTime.UtcNow.Ticks;
                            updateCommand.ExecuteNonQuery();
                            //TODO handle script error
                        }
                    }
                    return app;
                }
            }
        }
    }
}