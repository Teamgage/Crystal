using Crystal.DbProviders;
using Crystal.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Crystal.TestWebApp.DataAccess
{
    public class DesignTimeAppContextFactory : ShardedDbContextFactory<AppContext, long>
    {
        public DesignTimeAppContextFactory()
        : base(
            GetConnectionString(),
            new SqlServerDbProvider(),
            (manager, provider) => new AppContext(manager, provider))
        { }

        private static string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration.GetConnectionString("ModelDb");
        }
    }
}