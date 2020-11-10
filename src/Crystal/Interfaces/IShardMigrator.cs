namespace Crystal.Interfaces
{
    public interface IShardMigrator<TKey>
    {
        /// <summary>
        /// Updates a single shard to the latest database version
        /// </summary>
        /// <param name="key">The key of the shard to migrate</param>
        void MigrateShard(TKey key);
    }
}
