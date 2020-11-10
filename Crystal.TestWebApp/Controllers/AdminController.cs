﻿using Crystal.Interfaces;
using Crystal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crystal.TestWebApp.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IShardManager<long> _shardManager;
        private readonly IShardMigrator<long> _shardMigrator;

        public AdminController(IShardManager<long> shardManager, IShardMigrator<long> shardMigrator)
        {
            _shardManager = shardManager;
            _shardMigrator = shardMigrator;
        }

        [HttpGet]
        [Route("keys")]
        public IActionResult GetAllKeys()
        {
            return Ok(_shardManager.AllKeys);
        }

        [HttpPost]
        [Route("migrate/{id}")]
        public IActionResult MigrateShard(long id)
        {
            _shardMigrator.MigrateShard(id);
            return Ok();
        }

        [HttpPost]
        [Route("shards")]
        public IActionResult CreateShard(Shard<long> shard)
        {
            _shardManager.AddShard(shard);
            return Ok();
        }
    }
}
