using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Crystal.Interfaces
{
    public interface IDbProvider
    {
        /// <summary>
        /// Configures EFCore to use a specific database
        /// </summary>
        /// <param name="optionsBuilder">The EFCore options builder</param>
        /// <param name="dbName">The database name</param>
        void UseDatabase(DbContextOptionsBuilder optionsBuilder, string dbName);

        /// <summary>
        /// Migrates a database
        /// </summary>
        /// <param name="database">The database to migrate</param>
        void MigrateDatabase(DatabaseFacade database);
    }
}
