using System;
using Crystal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Crystal.Models
{
    public class ShardedDbContextFactory<TContext, TKey> : IDesignTimeDbContextFactory<TContext>
        where TContext : ShardedDbContext<TKey>
    {
        private readonly string _modelDbName;
        private readonly IDbProvider _dbProvider;
        private readonly Func<IShardManager<TKey>, IDbProvider, TContext> _contextFactory;

        public ShardedDbContextFactory(
            string modelDbName,
            IDbProvider dbProvider,
            Func<IShardManager<TKey>, IDbProvider, TContext> contextFactory)
        {
            _modelDbName = modelDbName;
            _dbProvider = dbProvider;
            _contextFactory = contextFactory;
        }

        public TContext CreateDbContext(string[] args)
        {
            var options = new ShardManagerOptions<TKey>
            {
                ModelDbName = _modelDbName
            };
            
            var shardManager = new ShardManager<TKey>(options);
            
            return _contextFactory.Invoke(shardManager, _dbProvider);
        }
    }
}