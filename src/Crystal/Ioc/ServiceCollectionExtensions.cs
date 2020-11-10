using System;
using Crystal.Interfaces;
using Crystal.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Crystal.Ioc
{
    public class CrystalServiceBuilder
    {
        public readonly IServiceCollection ServiceCollection;

        public CrystalServiceBuilder(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }
    }

    public static class ServiceCollectionExtensions
    {        
        public static CrystalServiceBuilder UseCrystal<TKey, TDbProvider>(
            this IServiceCollection serviceCollection,
            Action<ShardManagerOptions<TKey>> shardManagerOptionsBuilder)
            where TDbProvider : class, IDbProvider
        {
            var shardManagerOptions = new ShardManagerOptions<TKey>();

            shardManagerOptionsBuilder.Invoke(shardManagerOptions);
            VerifyOptions(shardManagerOptions);

            serviceCollection.AddSingleton(shardManagerOptions);
            serviceCollection.AddSingleton(shardManagerOptions.KeyExtractorDelegate);
            serviceCollection.AddTransient<IDbProvider, TDbProvider>();
            serviceCollection.AddScoped<IShardManager<TKey>, ShardManager<TKey>>();
            
            return new CrystalServiceBuilder(serviceCollection);
        }

        public static void WithShardMigrator<TKey, TContext>(
            this CrystalServiceBuilder serviceBuilder,
            Func<IShardManager<TKey>, IDbProvider, TContext> contextFactory)
            where TContext : ShardedDbContext<TKey>
        {
            serviceBuilder.ServiceCollection.AddSingleton(contextFactory);
            serviceBuilder.ServiceCollection.AddTransient<IShardMigrator<TKey>, ShardMigrator<TKey, TContext>>();
        }

        /// <summary>
        /// Gross method to validate everything, but someone's gotta do it
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <param name="options">The options to validate</param>
        private static void VerifyOptions<TKey>(ShardManagerOptions<TKey> options)
        {
            if (options.KeyExtractorDelegate == null)
            {
                throw new Exception("Key extractor must be set to use Crystal");
            }
            
            if (options.ShardStorageType == ShardStorageType.List)
            {
                if (options.Shards == null)
                {
                    throw new Exception("When using ShardStorageType.List, you must provide a list of shards");
                }
            }
        }
    }
}