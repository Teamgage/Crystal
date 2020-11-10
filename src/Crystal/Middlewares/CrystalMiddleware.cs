using Crystal.Exceptions;
using Crystal.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
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
            ShardManagerOptions<TKey> options,
            IShardManager<TKey> shardManager)
        {
            if (options.RouteExceptions.Any(context.Request.Path.Value.Contains))
            {
                await _next(context);
                return;
            }
            
            TKey key;

            try
            {
                key = options.KeyExtractorDelegate.Invoke(context);
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
