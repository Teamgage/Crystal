using System;
using Crystal.Interfaces;
using Microsoft.EntityFrameworkCore.Design;

namespace Crystal.Models
{
    public class ShardedDbContextFactory<TContext, TKey> : IDesignTimeDbContextFactory<TContext>
        where TContext : ShardedDbContext<TKey>
    {
        private readonly string _connectionString;
        private readonly IDbProvider _dbProvider;
        private readonly Func<IShardManager<TKey>, IDbProvider, TContext> _contextFactory;

        public ShardedDbContextFactory(
            string connectionString,
            IDbProvider dbProvider,
            Func<IShardManager<TKey>, IDbProvider, TContext> contextFactory)
        {
            _connectionString = connectionString;
            _dbProvider = dbProvider;
            _contextFactory = contextFactory;
        }

        public virtual TContext CreateDbContext(string[] args)
        {
            var options = new ShardManagerOptions<TKey>
            {
                ModelDbConnectionString = _connectionString
            };
            
            var shardManager = new ShardManager<TKey>(options);
            
            return _contextFactory.Invoke(shardManager, _dbProvider);
        }
    }
}