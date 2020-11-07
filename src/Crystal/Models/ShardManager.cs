using Crystal.Exceptions;
using Crystal.Interfaces;
using System;
using System.Collections.Concurrent;

namespace Crystal.Models
{
    public class ShardManager<TKey> : IShardManager<TKey>
    {
        private readonly ShardManagerOptions<TKey> _options;
        private readonly ConcurrentDictionary<TKey, Shard<TKey>> _shards;
        public TKey CurrentKey { get; private set; }
        
        public ShardManager()
            : this(null) { }

        public ShardManager(ShardManagerOptions<TKey> options)
        {
            _options = options;
            _shards = new ConcurrentDictionary<TKey, Shard<TKey>>();
        }
        
        public string CurrentDbName => _options.ModelDbName ?? _shards[CurrentKey].DbName;

        public void AddShard(Shard<TKey> shard)
        {
            if (!ValidateKey(shard.Key))
            {
                throw new ShardKeyException($"Invalid shard key provided. Key: {shard.Key}");
            }

            if (_shards.ContainsKey(shard.Key))
            {
                throw new ShardKeyException($"Shard with provided key already exists. Key: {shard.Key}");
            }

            _shards[shard.Key] = shard;
        }

        public void DeleteShard(TKey key)
        {
            if (!_shards.ContainsKey(key))
            {
                throw new ShardKeyException($"Shard with provided key does not exist. Key: {key}");
            }

            if (!_shards.TryRemove(key, out _))
            {
                throw new Exception($"Could not remove shard with key: {key}");
            }
        }

        public void SetCurrentShard(TKey key)
        {
            if (!ValidateKey(key) || !_shards.ContainsKey(key))
            {
                throw new ShardKeyException($"Invalid shard key provided. Key: {key}");
            }

            CurrentKey = key;
        }

        public bool ValidateKey(TKey key)
        {
            return _options?.KeyValidator == null || _options.KeyValidator.Invoke(key);
        }
    }
}
