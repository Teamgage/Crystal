using Crystal.Models;

namespace Crystal.Interfaces
{
    public interface IShardManager<TKey>
    {
        string CurrentDbName { get; }
        TKey CurrentKey { get; }
        void SetCurrentShard(TKey key);
        bool ValidateKey(TKey key);
        void AddShard(Shard<TKey> shard);
        void DeleteShard(TKey key);
    }
}
