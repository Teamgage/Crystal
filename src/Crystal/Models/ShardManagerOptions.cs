using Crystal.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Crystal.Models
{
    public class ShardManagerOptions<TKey>
    {
        /// <summary>
        /// The delegate used to extract the key from an incoming request
        /// </summary>
        public Func<HttpContext, TKey> KeyExtractorDelegate { get; set; }
        
        /// <summary>
        /// Any routes to ignore sharding for
        /// </summary>
        public ICollection<string> RouteExceptions { get; set; }

        /// <summary>
        /// A function to provide any custom key validation
        /// </summary>
        public Func<TKey, bool> KeyValidator { get; set; }

        /// <summary>
        /// A connection string for a model database
        /// </summary>
        public string ModelDbConnectionString { get; set; }

        /// <summary>
        /// The type of storage used for persisting shards
        /// </summary>
        public ShardStorageType? ShardStorageType { get; set; }

        /// <summary>
        /// If using the in-memory ShardStorageType, provides the initial shards
        /// </summary>
        public List<Shard<TKey>> Shards { get; set; }

        /// <summary>
        /// A connection string for a DB which holds a record of shards. Used with SQL-type
        /// ShardStorageTypes.
        /// </summary>
        public string ShardManagerDbConnectionString { get; set; }

        /// <summary>
        /// Used in conjunction with ShardStorageType.Custom, creates a shard storage provider
        /// </summary>
        public Func<IShardStorage<TKey>> ShardStorageFactory { get; set; }
    }

    public enum ShardStorageType
    {
        List,
        SqlServer,
        Custom
    }
}
