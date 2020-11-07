using Crystal.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Concurrent;

namespace Crystal.DbProviders
{
    public class SqliteMemoryDbProvider : IDbProvider, IDisposable
    {
        private readonly ConcurrentDictionary<string, SqliteConnection> _connections;

        public SqliteMemoryDbProvider()
        {
            _connections = new ConcurrentDictionary<string, SqliteConnection>();
        }

        public void MigrateDatabase(DatabaseFacade database)
        {
            database.EnsureCreated();
        }

        public void UseDatabase(DbContextOptionsBuilder optionsBuilder, string connectionString)
        {
            if (!_connections.ContainsKey(connectionString))
            {
                var conn = new SqliteConnection(ConnectionString(connectionString));
                _connections[connectionString] = conn;
                conn.Open();
            }

            optionsBuilder.UseSqlite(ConnectionString(connectionString));
        }

        public void Dispose()
        {
            foreach (var conn in _connections.Values)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private string ConnectionString(string connectionString) => new SqliteConnectionStringBuilder
        {
            DataSource = $"TestDb-{connectionString}",
            Mode = SqliteOpenMode.Memory,
            Cache = SqliteCacheMode.Shared
        }.ToString();
    }
}
