using Crystal.DbProviders;
using Crystal.Models;

namespace Crystal.TestWebApp.DataAccess
{
    public class DesignTimeAppContextFactory : ShardedDbContextFactory<AppContext, long>
    {
        public DesignTimeAppContextFactory()
        : base(
            string.Empty,
            new SqlServerDbProvider(string.Empty),
            (manager, provider) => new AppContext(manager, provider))
        { }
    }
}