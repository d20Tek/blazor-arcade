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
            var id = "test-profile-id-1";
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
            var id = "test-profile-id-1";
            var id2 = "test-profile-id-2";
            var userId = "test-user-1";
            var list = new List<UserProfileEntity>
            {
                new UserProfileEntity { ProfileId = id, UserId = userId },
                new UserProfileEntity { ProfileId = id2, UserId = userId },
            };
            var cache = new MemoryCacheService();

            // act 1: add cached list
            var result = cache.SetList<UserProfileEntity>(userId, list);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.ProfileId == id));
            Assert.IsTrue(result.Any(p => p.ProfileId == id2));

            // act 2: contains cached list
            var contains = cache.ContainsList<UserProfileEntity>(userId);
            Assert.IsTrue(contains);

            // act 3: get cached list
            result = cache.GetList<UserProfileEntity>(userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.ProfileId == id));
            Assert.IsTrue(result.Any(p => p.ProfileId == id2));

            // act 4: remove cached list
            result = cache.RemoveList<UserProfileEntity>(userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.ProfileId == id));
            Assert.IsTrue(result.Any(p => p.ProfileId == id2));

            // act 5: check list no longer contained.
            contains = cache.ContainsList<UserProfileEntity>(userId);
            Assert.IsFalse(contains);

            result = cache.GetList<UserProfileEntity>(userId);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void MemoryCache_SetGetMixed()
        {
            // arrange
            var id = "test-profile-id-1";
            var id2 = "test-profile-id-2";
            var id3 = "test-profile-id-3";
            var userId = "test-user-1";
            var list = new List<UserProfileEntity>
            {
                new UserProfileEntity { ProfileId = id, UserId = userId },
                new UserProfileEntity { ProfileId = id2, UserId = userId },
            };
            var profile3 = new UserProfileEntity { ProfileId = id3, UserId = userId };

            var cache = new MemoryCacheService();

            // act 1: add cached list
            var result = cache.SetList<UserProfileEntity>(userId, list);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.ProfileId == id));
            Assert.IsTrue(result.Any(p => p.ProfileId == id2));

            // act 2: contains cached list
            var contains = cache.ContainsList<UserProfileEntity>(userId);
            Assert.IsTrue(contains);

            // act 3: add new entity
            var r3 = cache.Set(id3, profile3);

            result = cache.GetList<UserProfileEntity>(userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Any(p => p.ProfileId == id));
            Assert.IsTrue(result.Any(p => p.ProfileId == id2));
            Assert.IsTrue(result.Any(p => p.ProfileId == id3));

            // act 4: update entity
            profile3.DisplayName = "updated";
            _ = cache.Set<UserProfileEntity>(id3, profile3);

            result = cache.GetList<UserProfileEntity>(userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Any(p => p.ProfileId == id));
            Assert.IsTrue(result.Any(p => p.ProfileId == id2));
            Assert.IsTrue(result.Any(p => p.ProfileId == id3));
            Assert.AreEqual("updated", result.Last().DisplayName);

            // act 5: remove entity
            _ = cache.Remove<UserProfileEntity>(id3);

            result = cache.GetList<UserProfileEntity>(userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.ProfileId == id));
            Assert.IsTrue(result.Any(p => p.ProfileId == id2));
            Assert.IsFalse(result.Any(p => p.ProfileId == id3));
        }
    }
}
