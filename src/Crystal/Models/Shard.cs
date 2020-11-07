namespace Crystal.Models
{
    public class Shard<TKey>
    {
        public TKey Key { get; }
        public string ConnectionString { get; }

        public Shard(TKey key, string connectionString)
        {
            Key = key;
            ConnectionString = connectionString;
        }
    }
}
