using System;

namespace Crystal.Models
{
    public class ShardManagerOptions<TKey>
    {
        public Func<TKey, bool> KeyValidator { get; set; }
        public string ModelDbName { get; set; }
    }
}
