namespace Crystal.Models
{
    public class Shard<TKey>
    {
        public TKey Key { get; }
        public string DbName { get; }

        public Shard(TKey key, string dbName)
        {
            Key = key;
            DbName = dbName;
        }
    }
}
