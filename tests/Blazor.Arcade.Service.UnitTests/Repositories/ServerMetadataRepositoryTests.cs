//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Service.Repositories;

namespace Blazor.Arcade.Service.UnitTests.Repositories
{
    [TestClass]
    public class ServerMetadataRepositoryTests
    {
        [TestMethod]
        public async Task GetAll()
        {
            // arrange
            var repo = new ServerMetadataRepository();

            // act
            var metadata = await repo.GetAll();

            // assert
            Assert.IsNotNull(metadata);
            Assert.AreEqual(1, metadata.Count);
            Assert.IsTrue(metadata.Any(p => p.Name == "s1"));
        }

        [TestMethod]
        public async Task GetById()
        {
            // arrange
            var repo = new ServerMetadataRepository();

            // act
            var metadata = await repo.GetById("s1");

            // assert
            Assert.IsNotNull(metadata);
            if (metadata != null)
            {
                Assert.AreEqual("001", metadata.Prefix);
                Assert.AreEqual("s1", metadata.Name);
            }
        }
    }
}
