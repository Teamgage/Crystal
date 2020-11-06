using Crystal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Crystal.DbProviders
{
    public class SqlServerDbProvider : IDbProvider
    {
        private readonly string _baseConnectionString;

        public SqlServerDbProvider(string baseConnectionString)
        {
            _baseConnectionString = baseConnectionString;
        }

        public void MigrateDatabase(DatabaseFacade database)
        {
            database.Migrate();
        }

        public void UseDatabase(DbContextOptionsBuilder optionsBuilder, string dbName)
        {
            optionsBuilder.UseSqlServer($"{_baseConnectionString}Databases={dbName}");
        }
    }
}
