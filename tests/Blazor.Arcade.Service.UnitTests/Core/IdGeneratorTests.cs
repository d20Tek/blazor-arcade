//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

using Blazor.Arcade.Common.Core;

namespace Blazor.Arcade.Service.UnitTests.Core
{
    [TestClass]
    public class IdGeneratorTests
    {
        [TestMethod]
        public void GetNext()
        {
            // arrange

            // act
            var id = IdGenerator.Instance.GetNext();

            // assert
            Assert.IsNotNull(id);
            Assert.AreEqual(9, id.Length);
        }

        [TestMethod]
        public void GetNext_WithPrefix()
        {
            // arrange
            var prefix = "201";

            // act
            var id = IdGenerator.Instance.GetNext(prefix);

            // assert
            Assert.IsNotNull(id);
            Assert.AreEqual(13, id.Length);
            Assert.IsTrue(id.StartsWith($"{prefix}-"));
        }
    }
}
