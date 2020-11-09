using Crystal.Models;
using System.Collections.Generic;

namespace Crystal.Interfaces
{
    public interface IShardStorage<TKey>
    {
        /// <summary>
        /// Gets a list of any initial shards
        /// </summary>
        /// <returns>Any existing shards</returns>
        IEnumerable<Shard<TKey>> Load();

        /// <summary>
        /// Adds a shard to the store
        /// </summary>
        /// <param name="shard">The shard to add</param>
        void Add(Shard<TKey> shard);

        /// <summary>
        /// Removes a shard from the store by it's key
        /// </summary>
        /// <param name="key">The key of the shard to remove</param>
        void Remove(TKey key);
    }
}
