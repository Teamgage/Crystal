using Crystal.Interfaces;
using System;

namespace Crystal.Models
{
    public class ShardMigrator<TKey, TContext> : IShardMigrator<TKey>
        where TContext : ShardedDbContext<TKey>
    {
        private readonly IShardManager<TKey> _shardManager;
        private readonly IDbProvider _dbProvider;
        private readonly Func<IShardManager<TKey>, IDbProvider, TContext> _contextFactory;

        public ShardMigrator(
            IShardManager<TKey> shardManager,
            IDbProvider dbProvider,
            Func<IShardManager<TKey>, IDbProvider, TContext> contextFactory)
        {
            _shardManager = shardManager;
            _dbProvider = dbProvider;
            _contextFactory = contextFactory;
        }

        public void MigrateShard(TKey key)
        {
            var context = _contextFactory.Invoke(_shardManager, _dbProvider);
            
            _shardManager.SetCurrentShard(key);
            _dbProvider.MigrateDatabase(context.Database);
        }
    }
}
