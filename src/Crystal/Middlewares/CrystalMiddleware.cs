using Crystal.Exceptions;
using Crystal.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Crystal.Models;

namespace Crystal.Middlewares
{
    public class CrystalMiddleware<TKey>
    {
        private readonly RequestDelegate _next;

        public CrystalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext context,
            ShardKeyExtractor<TKey> keyExtractor,
            IShardManager<TKey> shardManager)
        {
            TKey key;

            try
            {
                key = keyExtractor.Delegate.Invoke(context);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("No tenant key provided");
                return;
            }

            if (!shardManager.ValidateKey(key))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync($"Invalid tenant key provided: {key}");
                return;
            }

            try
            {
                shardManager.SetCurrentShard(key);
                await _next(context);
            }
            catch (ShardKeyException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync($"Could not set current tenant to key: {key}");
            }
        }
    }
}
