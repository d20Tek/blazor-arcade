//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;

namespace Blazor.Arcade.Service.UnitTests.Core
{
    [TestClass]
    public class ExceptionTests
    {
        private readonly Exception _inner = new Exception("inner exception");

        [TestMethod]
        public void EntityAlreadyExistsExceptionTest()
        {
            // arrange

            // act
            var ex = new EntityAlreadyExistsException("entity", "test", _inner);

            // assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("entity", ex.EntityIdName);
            Assert.AreEqual("test", ex.EntityIdValue);
        }

        [TestMethod]
        public void EntityNotFoundExceptionTest()
        {
            // arrange

            // act
            var ex = new EntityNotFoundException("entity", "test", _inner);

            // assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("entity", ex.EntityIdName);
            Assert.AreEqual("test", ex.EntityIdValue);
        }
    }
}
