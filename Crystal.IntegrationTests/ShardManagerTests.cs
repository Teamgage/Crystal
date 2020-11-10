using Crystal.Exceptions;
using Crystal.Interfaces;
using Crystal.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Crystal.IntegrationTests
{
    [TestClass]
    public class ShardManagerTests
    {
        private IShardManager<long> _sut;

        [TestInitialize]
        public void SetUp()
        {
            _sut = new ShardManager<long>(new ShardManagerOptions<long>());
        }

        [TestMethod]
        public void AddShard_ExistingShard_ExceptionThrown()
        {
            var shardOne = new Shard<long>(1, "dbOne");
            var shardTwo = new Shard<long>(1, "dbTwo");

            _sut.AddShard(shardOne);

            Assert.ThrowsException<ShardKeyException>(() => _sut.AddShard(shardTwo));
        }

        [TestMethod]
        public void DeleteShard_NonExistentShard_ExceptionThrown()
        {
            Assert.ThrowsException<ShardKeyException>(() => _sut.DeleteShard(1));
        }

        [TestMethod]
        public void ValidateKey_CustomValidator_ValidatorUsed()
        {
            _sut = new ShardManager<long>(new ShardManagerOptions<long>
            {
                KeyValidator = (long key) => key < 5
            });

            Assert.IsFalse(_sut.ValidateKey(5));
        }

        [TestMethod]
        public void AddShard_CustomValidator_InvalidShardThrowsException()
        {
            _sut = new ShardManager<long>(new ShardManagerOptions<long>
            {
                KeyValidator = (long key) => key < 5
            });

            var shard = new Shard<long>(5, "dbOne");

            Assert.ThrowsException<ShardKeyException>(() => _sut.AddShard(shard));
        }

        [TestMethod]
        public void SetCurrentShard_ValidShard_CurrentShardUpdated()
        {
            var shard = new Shard<long>(1, ("dbOne"));

            _sut.AddShard(shard);
            _sut.SetCurrentShard(1);

            Assert.AreEqual(1, _sut.CurrentKey);
            Assert.AreEqual("dbOne", _sut.CurrentConnectionString);
        }

        [TestMethod]
        public void SetCurrentShard_NonExistentKey_ExceptionThrown()
        {
            Assert.ThrowsException<ShardKeyException>(() => _sut.SetCurrentShard(1));
        }
    }
}
