//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Service.Entities;
using Blazor.Arcade.Service.UnitTests.Mocks;

namespace Blazor.Arcade.Service.UnitTests.Core
{
    [TestClass]
    public class MemoryCacheServiceTests
    {
        [TestMethod]
        public void MemoryCache_SetGetEntity()
        {
            // arrange
            var id = "test-account-id-1";
            var entity = new TestEntity { TestId = id };
            var cache = new MemoryCacheService();

            // act 1: add cached entity
            var result = cache.Set<TestEntity>(id, entity);
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.TestId);

            // act 2: contains cached entity
            var contains = cache.Contains<TestEntity>(id);
            Assert.IsTrue(contains);

            // act 3: get cached entity
            result = cache.Get<TestEntity>(id);
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.TestId);

            // act 4: remove cached entity
            result = cache.Remove<TestEntity>(id);
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.TestId);

            // act 5: check entity no longer contained.
            contains = cache.Contains<TestEntity>(id);
            Assert.IsFalse(contains);

            result = cache.Get<TestEntity>(id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void MemoryCache_SetGetList()
        {
            // arrange
            var id = "test-account-id-1";
            var id2 = "test-account-id-2";
            var userId = "test-user-1";
            var list = new List<UserAccountEntity>
            {
                new UserAccountEntity { AccountId = id, UserId = userId },
                new UserAccountEntity { AccountId = id2, UserId = userId },
            };
            var cache = new MemoryCacheService();

            // act 1: add cached list
            var result = cache.SetList<UserAccountEntity>(userId, list);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.AccountId == id));
            Assert.IsTrue(result.Any(p => p.AccountId == id2));

            // act 2: contains cached list
            var contains = cache.ContainsList<UserAccountEntity>(userId);
            Assert.IsTrue(contains);

            // act 3: get cached list
            result = cache.GetList<UserAccountEntity>(userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.AccountId == id));
            Assert.IsTrue(result.Any(p => p.AccountId == id2));

            // act 4: remove cached list
            result = cache.RemoveList<UserAccountEntity>(userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.AccountId == id));
            Assert.IsTrue(result.Any(p => p.AccountId == id2));

            // act 5: check list no longer contained.
            contains = cache.ContainsList<UserAccountEntity>(userId);
            Assert.IsFalse(contains);

            result = cache.GetList<UserAccountEntity>(userId);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void MemoryCache_SetGetMixed()
        {
            // arrange
            var id = "test-account-id-1";
            var id2 = "test-account-id-2";
            var id3 = "test-account-id-3";
            var userId = "test-user-1";
            var list = new List<UserAccountEntity>
            {
                new UserAccountEntity { AccountId = id, UserId = userId },
                new UserAccountEntity { AccountId = id2, UserId = userId },
            };
            var account3 = new UserAccountEntity { AccountId = id3, UserId = userId };

            var cache = new MemoryCacheService();

            // act 1: add cached list
            var result = cache.SetList<UserAccountEntity>(userId, list);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.AccountId == id));
            Assert.IsTrue(result.Any(p => p.AccountId == id2));

            // act 2: contains cached list
            var contains = cache.ContainsList<UserAccountEntity>(userId);
            Assert.IsTrue(contains);

            // act 3: add new entity
            var r3 = cache.Set(id3, account3);

            result = cache.GetList<UserAccountEntity>(userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Any(p => p.AccountId == id));
            Assert.IsTrue(result.Any(p => p.AccountId == id2));
            Assert.IsTrue(result.Any(p => p.AccountId == id3));

            // act 4: update entity
            account3.DisplayName = "updated";
            _ = cache.Set<UserAccountEntity>(id3, account3);

            result = cache.GetList<UserAccountEntity>(userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Any(p => p.AccountId == id));
            Assert.IsTrue(result.Any(p => p.AccountId == id2));
            Assert.IsTrue(result.Any(p => p.AccountId == id3));
            Assert.AreEqual("updated", result.Last().DisplayName);

            // act 5: remove entity
            _ = cache.Remove<UserAccountEntity>(id3);

            result = cache.GetList<UserAccountEntity>(userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.AccountId == id));
            Assert.IsTrue(result.Any(p => p.AccountId == id2));
            Assert.IsFalse(result.Any(p => p.AccountId == id3));
        }
    }
}
