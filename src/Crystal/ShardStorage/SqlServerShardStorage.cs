using Crystal.Interfaces;
using Crystal.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Crystal.ShardStorage
{
    public class SqlServerShardStorage<TKey> : IShardStorage<TKey>
    {
        private readonly string _connectionString;

        public SqlServerShardStorage(string connectionString)
        {
            _connectionString = connectionString;

            EnsureTableCreated();
        }

        public IEnumerable<Shard<TKey>> Load()
        {
            const string query = "SELECT * FROM[dbo].[__shards]";
            var result = new List<Shard<TKey>>();

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var key = (TKey)reader["Key"];
                            result.Add(new Shard<TKey>(key, (string)reader["ConnectionString"]));
                        }
                    }
                }
            }

            return result;
        }

        public void Add(Shard<TKey> shard)
        {
            const string query = @"
                INSERT INTO [dbo].[__shards] (
                    [Key],
                    [ConnectionString]
                ) VALUES (
                    @key,
                    @connectionString
                )";

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@key", shard.Key);
                    command.Parameters.AddWithValue("@connectionString", shard.ConnectionString);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Remove(TKey key)
        {
            const string query = @"DELET FROM [dbo].[__shards] WHERE [Key] = @key";

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@key", key);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void EnsureTableCreated()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                string keyType;

                if (typeof(TKey) == typeof(int))
                {
                    keyType = "INT";
                }
                else if (typeof(TKey) == typeof(long))
                {
                    keyType = "BIGINT";
                }
                else if (typeof(TKey) == typeof(string))
                {
                    keyType = "VARCHAR";
                }
                else
                {
                    throw new Exception("Only key types of int, long and string are accepted");
                }

                var ensureTableCreatedCommand = $@"
                    IF NOT EXISTS (SELECT * from [dbo].[sysobjects] WHERE [name]='__shards' AND [xtype]='U')
                        CREATE TABLE [dbo].[__shards] (
                            [Key] {keyType} NOT NULL,
                            [ConnectionString] VARCHAR NOT NULL
                        )";

                using (var command = new SqlCommand(ensureTableCreatedCommand))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
