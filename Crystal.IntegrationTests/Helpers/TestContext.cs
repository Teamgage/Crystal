using Crystal.Interfaces;
using Crystal.Models;
using Microsoft.EntityFrameworkCore;

namespace Crystal.IntegrationTests.Helpers
{
    public class TestContext : ShardedDbContext<long>
    {
        public TestContext(IShardManager<long> shardManager, IDbProvider dbProvider)
            : base(shardManager, dbProvider) { }

        public DbSet<MockEntity> MockEntities { get; set; }
    }
}
