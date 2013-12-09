using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace VacanciesIndexing.Database
{
    class DatabaseDeployer
    {
        private static readonly string _st_desiredConnectionString;

        private const string _c_checkDbExistencePattern = "SELECT DB_ID('{0}')";
        private const string _c_createDbPattern = "CREATE DATABASE {0}";

        static DatabaseDeployer()
        {
            _st_desiredConnectionString = IndexingService.ActualConfiguration.ConnectionString;
        }

        public static void PrepareDatabase()
        {
            IndexingService.Log.Info("Preparing database started");

            CreateDatabaseIfNotExists();
            CreateDatabaseObjects();

            IndexingService.Log.Info("Preparing database finished");
        }

        private static void CreateDatabaseIfNotExists()
        {
            var dbName = new SqlConnection(_st_desiredConnectionString).Database;
            IndexingService.Log.Info(string.Format("Desired database name is '{0}'", dbName));

            var connStrToMaster = new SqlConnectionStringBuilder(_st_desiredConnectionString)
            {
                InitialCatalog = "master"
            };

            using (var connectionToMaster = new SqlConnection(connStrToMaster.ConnectionString))
            {
                var checkDbExistenceText = string.Format(_c_checkDbExistencePattern, dbName);
                var checkDbExistenceCommand = new SqlCommand(checkDbExistenceText, connectionToMaster);
                
                connectionToMaster.Open();

                bool dbNotExists = checkDbExistenceCommand.ExecuteScalar() is DBNull;
                if (dbNotExists)
                {
                    IndexingService.Log.Debug(string.Format("Database '{0}' not exists", dbName));

                    var createDbText = string.Format(_c_createDbPattern, dbName);
                    var createDbCommand = new SqlCommand(createDbText, connectionToMaster);
                    createDbCommand.ExecuteNonQuery();

                    IndexingService.Log.Debug(string.Format("Database '{0}' created", dbName));
                }
                else
                {
                    IndexingService.Log.Debug(string.Format("Database '{0}' already exists", dbName));
                }
            }
        }

        private static void CreateDatabaseObjects()
        {
            using (var connection = new SqlConnection(_st_desiredConnectionString))
            {
                IndexingService.Log.Debug("Creating database objects started");

                var pathToScript = IndexingService.ActualConfiguration.PathToCreatingObjectsScript;
                var script = File.ReadAllText(pathToScript);

                var server = new Server(new ServerConnection(connection));
                server.ConnectionContext.ExecuteNonQuery(script);

                IndexingService.Log.Debug("Creating database objects finished");
            }
        }
    }
}
