using System;
using Microsoft.AspNetCore.Http;

namespace Crystal.Models
{
    public class ShardKeyExtractor<TKey>
    {
        public Func<HttpContext, TKey> Delegate { get; set; }
    }
}