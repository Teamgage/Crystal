using Crystal.Interfaces;
using Crystal.Models;
using System.Collections.Generic;

namespace Crystal.ShardStorage
{
    public class ListShardStorage<TKey> : IShardStorage<TKey>
    {
        private readonly List<Shard<TKey>> _shards;

        public ListShardStorage(List<Shard<TKey>> shards)
        {
            _shards = shards;
        }

        public void Add(Shard<TKey> shard)
        {
            _shards.Add(shard);
        }

        public IEnumerable<Shard<TKey>> Load()
        {
            return _shards;
        }

        public void Remove(TKey key)
        {
            // @TODO
        }
    }
}
