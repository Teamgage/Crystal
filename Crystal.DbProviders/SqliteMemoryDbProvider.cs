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

        public void UseDatabase(DbContextOptionsBuilder optionsBuilder, string dbName)
        {
            if (!_connections.ContainsKey(dbName))
            {
                var conn = new SqliteConnection(ConnectionString(dbName));
                _connections[dbName] = conn;
                conn.Open();
            }

            optionsBuilder.UseSqlite(ConnectionString(dbName));
        }

        public void Dispose()
        {
            foreach (var conn in _connections.Values)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private string ConnectionString(string dbName) => new SqliteConnectionStringBuilder
        {
            DataSource = $"TestDb-{dbName}",
            Mode = SqliteOpenMode.Memory,
            Cache = SqliteCacheMode.Shared
        }.ToString();
    }
}
