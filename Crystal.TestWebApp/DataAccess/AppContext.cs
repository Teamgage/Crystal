using Crystal.Interfaces;
using Crystal.Models;
using Crystal.TestWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Crystal.TestWebApp.DataAccess
{
    public class AppContext : ShardedDbContext<long>
    {
        public DbSet<Entity> Entities { get; set; }
        
        public AppContext(IShardManager<long> shardManager, IDbProvider dbProvider)
            : base(shardManager, dbProvider) { }
    }
}