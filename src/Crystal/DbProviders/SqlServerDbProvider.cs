using Crystal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Crystal.DbProviders
{
    public class SqlServerDbProvider : IDbProvider
    {
        public void MigrateDatabase(DatabaseFacade database)
        {
            database.Migrate();
        }

        public void UseDatabase(DbContextOptionsBuilder optionsBuilder, string connectionString)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
