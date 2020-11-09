using Crystal.Interfaces;
using System;
using System.Collections.Generic;

namespace Crystal.Models
{
    public class ShardManagerOptions<TKey>
    {
        public Func<TKey, bool> KeyValidator { get; set; }
        public string ModelDbConnectionString { get; set; }
        public ShardStorageType? ShardStorageType { get; set; }
        public IList<Shard<TKey>> InitialShards { get; set; }
        public string ShardManagerDbConnectionString { get; set; }
        public Func<IShardStorage<TKey>> ShardStorageFactory { get; set; }
    }

    public enum ShardStorageType
    {
        List,
        SqlServer,
        Custom
    }
}
