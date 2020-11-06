using Crystal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Crystal.Models
{
    public class ShardedDbContext<TKey> : DbContext
    {
        private readonly IShardManager<TKey> _shardManager;
        private readonly IDbProvider _dbProvider;

        protected ShardedDbContext(IShardManager<TKey> shardManager, IDbProvider dbProvider)
        {
            _shardManager = shardManager;
            _dbProvider = dbProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _dbProvider.UseDatabase(optionsBuilder, _shardManager.CurrentDbName);
        }
    }
}
