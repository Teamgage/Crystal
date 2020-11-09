using System;
using Crystal.Interfaces;
using Crystal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Crystal.Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseCrystal<TKey>(
            this IServiceCollection serviceCollection,
            Func<HttpContext, TKey> keyExtractor) => UseCrystal(serviceCollection, keyExtractor, null);
        
        public static IServiceCollection UseCrystal<TKey>(
            this IServiceCollection serviceCollection,
            Func<HttpContext, TKey> keyExtractor,
            ShardManagerOptions<TKey> shardManagerOptions)
        {
            var shardKeyExtractor = new ShardKeyExtractor<TKey>
            {
                Delegate = keyExtractor
            };

            if (shardManagerOptions == null)
            {
                serviceCollection.AddSingleton(shardManagerOptions);
            }

            serviceCollection.AddSingleton(shardKeyExtractor);
            serviceCollection.AddScoped<IShardManager<TKey>, ShardManager<TKey>>();
            
            return serviceCollection;
        }
    }
}