using System.Collections.Generic;
using Crystal.Models;

namespace Crystal.Interfaces
{
    public interface IShardManager<TKey>
    {
        /// <summary>
        /// The connection string linked to the currently active shard
        /// </summary>
        string CurrentConnectionString { get; }

        /// <summary>
        /// The currently active shard key
        /// </summary>
        TKey CurrentKey { get; }
        
        /// <summary>
        /// Gets all currently registered shard keys
        /// </summary>
        IEnumerable<TKey> AllKeys { get; }

        /// <summary>
        /// Updates the currently active shard
        /// </summary>
        /// <param name="key">The key of the shard to switch ti</param>
        void SetCurrentShard(TKey key);

        /// <summary>
        /// Determines whether a key is valid
        /// </summary>
        /// <param name="key">The key to validate</param>
        /// <returns>True if the key is valid</returns>
        bool ValidateKey(TKey key);

        /// <summary>
        /// Adds a new shard to the system
        /// </summary>
        /// <param name="shard">The shard to add</param>
        void AddShard(Shard<TKey> shard);

        /// <summary>
        /// Deletes a shard from the system
        /// </summary>
        /// <param name="key">The key of the shard to delete</param>
        void DeleteShard(TKey key);
    }
}
