using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace SKS_Service_Manager
{
    public class DatabaseSynchronizer
    {
        private DataBase database;
        private List<string> operationBuffer;
        private List<(string table, int id)> deletedRecordsBuffer;
        private Logger logger;

        public DatabaseSynchronizer(DataBase db, Logger log)
        {
            database = db;
            operationBuffer = new List<string>();
            deletedRecordsBuffer = new List<(string table, int id)>();
            logger = log;
        }

        public void BufferOperation(string sql)
        {
            operationBuffer.Add(sql);
        }

        public void BufferDeleteOperation(string table, int id)
        {
            deletedRecordsBuffer.Add((table, id));
        }

        public async Task SynchronizeAsync()
        {
            if (database.IsMySQLConnectionAvailable(null))
            {
                using (var remoteConnection = database.GetMySqlConnection())
                {
                    await remoteConnection.OpenAsync();
                    using (var localConnection = database.GetSQLiteConnection())
                    {
                        await localConnection.OpenAsync();
                        await FetchAndSyncFromRemote(localConnection, remoteConnection);
                        await SyncDeletedRecords(remoteConnection, localConnection);
                    }
                }
            }

            using (var localConnection = database.GetSQLiteConnection())
            {
                await localConnection.OpenAsync();
                foreach (var sql in operationBuffer)
                {
                    var command = new SQLiteCommand(sql, localConnection);
                    await command.ExecuteNonQueryAsync();
                }
                operationBuffer.Clear();
            }
        }

        private async Task FetchAndSyncFromRemote(SQLiteConnection localConnection, MySqlConnection remoteConnection)
        {
            await FetchTableData(localConnection, remoteConnection, "Users");
            await FetchTableData(localConnection, remoteConnection, "UKS");
        }

        private async Task FetchTableData(SQLiteConnection localConnection, MySqlConnection remoteConnection, string tableName)
        {
            string query = $"SELECT * FROM {tableName}";
            MySqlCommand remoteCommand = new MySqlCommand(query, remoteConnection);
            using (var reader = await remoteCommand.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var columns = new List<string>();
                    var values = new List<object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        columns.Add(reader.GetName(i));
                        values.Add(reader.GetValue(i));
                    }

                    var insertColumns = string.Join(", ", columns);
                    var insertValues = string.Join(", ", columns.ConvertAll(c => $"@{c}"));

                    string insertQuery = $@"
                        INSERT OR IGNORE INTO {tableName} ({insertColumns})
                        VALUES ({insertValues});";

                    SQLiteCommand localCommand = new SQLiteCommand(insertQuery, localConnection);
                    for (int i = 0; i < columns.Count; i++)
                    {
                        localCommand.Parameters.AddWithValue($"@{columns[i]}", values[i]);
                    }

                    await localCommand.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task SyncDeletedRecords(MySqlConnection remoteConnection, SQLiteConnection localConnection)
        {
            // Synchronizacja usuniętych rekordów z bazy MySQL do SQLite
            foreach (var (table, id) in deletedRecordsBuffer)
            {
                string deleteQuery = $"DELETE FROM {table} WHERE ID = @ID";
                MySqlCommand remoteCommand = new MySqlCommand(deleteQuery, remoteConnection);
                remoteCommand.Parameters.AddWithValue("@ID", id);
                await remoteCommand.ExecuteNonQueryAsync();

                SQLiteCommand localCommand = new SQLiteCommand(deleteQuery, localConnection);
                localCommand.Parameters.AddWithValue("@ID", id);
                await localCommand.ExecuteNonQueryAsync();
            }
            deletedRecordsBuffer.Clear();
        }
    }
}
