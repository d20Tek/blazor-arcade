//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Service.UnitTests.Mocks;

namespace Blazor.Arcade.Service.UnitTests.Core
{
    [TestClass]
    public class CosmosStoreEntityTests
    {
        [TestMethod]
        public void Create()
        {
            // arrange

            // act
            var entity = new TestEntity
            {
                TestId = "foo-id",
                Attachments = "/attach",
                ETag = "test-etag",
                RowId = "0001",
                Self = "/self",
                Timestamp = 101010
            };

            // assert
            Assert.IsNotNull(entity);
            Assert.AreEqual("foo-id", entity.TestId);
            Assert.AreEqual("foo-id", entity.Id);
            Assert.AreEqual("foo-id", entity.PartitionId);
            Assert.AreEqual("/attach", entity.Attachments);
            Assert.AreEqual("test-etag", entity.ETag);
            Assert.AreEqual("0001", entity.RowId);
            Assert.AreEqual("/self", entity.Self);
            Assert.AreEqual(101010, entity.Timestamp);

        }
    }
}
