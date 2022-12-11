//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Service.Controllers;
using Blazor.Arcade.Service.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blazor.Arcade.Service.UnitTests.Controllers
{
    [TestClass]
    public class ArcadeControllerBaseTests
    {
        public sealed class TestController : ArcadeControllerBase
        {
            public TestController()
                : base(new Mock<ILogger<TestController>>().Object)
            {
            }

            public ActionResult<bool> Succeed()
            {
                return EndpointOperation<bool>("Succeed", () =>
                {
                    return true;
                });
            }

            public ActionResult<bool> MessageFormatError()
            {
                return EndpointOperation<bool>("MessageFormatError", () =>
                {
                    throw new ArgumentException("test-arg");
                });
            }

            public ActionResult<bool> UnexpectedError()
            {
                return EndpointOperation<bool>("UnexpectedError", () =>
                {
                    throw new InvalidOperationException();
                });
            }

            public async Task<ActionResult<bool>>SucceedAsync()
            {
                return await EndpointOperationAsync<bool>("Succeed", () =>
                {
                    return Task.FromResult<ActionResult<bool>>(true);
                });
            }

            public async Task<ActionResult<bool>> MessageFormatErrorAsync()
            {
                return await EndpointOperationAsync<bool>("MessageFormatError", () =>
                {
                    throw new ArgumentException("test-arg");
                });
            }

            public async Task<ActionResult<bool>> UnexpectedErrorAsync()
            {
                return await EndpointOperationAsync<bool>("UnexpectedError", () =>
                {
                    throw new InvalidOperationException();
                });
            }

            public async Task<ActionResult<bool>> NotFoundErrorAsync()
            {
                return await EndpointOperationAsync<bool>("NotFoundError", () =>
                {
                    throw new EntityNotFoundException("test", "value");
                });
            }

            public async Task<ActionResult<bool>> AlreadyExistsErrorAsync()
            {
                return await EndpointOperationAsync<bool>("AlreadyExistsError", () =>
                {
                    throw new EntityAlreadyExistsException("test", "value");
                });
            }
        }

        [TestMethod]
        public void EndpointOperation_Success()
        {
            // arrange
            var c = new TestController();

            // act
            var result = c.Succeed();

            // assert
            Assert.IsTrue(result.Value);
            ObjectResultValidation.AssertSuccess(result);
        }

        [TestMethod]
        public void EndpointOperation_MessageFormatError()
        {
            // arrange
            var c = new TestController();

            // act
            var result = c.MessageFormatError();

            // assert
            Assert.IsNotNull(result);
            ObjectResultValidation.AssertStatusCode(StatusCodes.Status422UnprocessableEntity, result);
        }

        [TestMethod]
        public void EndpointOperation_UnexpectedError()
        {
            // arrange
            var c = new TestController();

            // act
            var result = c.UnexpectedError();

            // assert
            Assert.IsNotNull(result);
            ObjectResultValidation.AssertStatusCode(StatusCodes.Status500InternalServerError, result);
        }

        [TestMethod]
        public async Task EndpointOperationAsync_Success()
        {
            // arrange
            var c = new TestController();

            // act
            var result = await c.SucceedAsync();

            // assert
            Assert.IsTrue(result.Value);
            ObjectResultValidation.AssertSuccess(result);
        }

        [TestMethod]
        public async Task EndpointOperationAsync_MessageFormatError()
        {
            // arrange
            var c = new TestController();

            // act
            var result = await c.MessageFormatErrorAsync();

            // assert
            Assert.IsNotNull(result);
            ObjectResultValidation.AssertStatusCode(StatusCodes.Status422UnprocessableEntity, result);
        }

        [TestMethod]
        public async Task EndpointOperationAsync_UnexpectedError()
        {
            // arrange
            var c = new TestController();

            // act
            var result = await c.UnexpectedErrorAsync();

            // assert
            Assert.IsNotNull(result);
            ObjectResultValidation.AssertStatusCode(StatusCodes.Status500InternalServerError, result);
        }

        [TestMethod]
        public async Task EndpointOperationAsync_AlreadyExistsError()
        {
            // arrange
            var c = new TestController();

            // act
            var result = await c.AlreadyExistsErrorAsync();

            // assert
            Assert.IsNotNull(result);
            ObjectResultValidation.AssertStatusCode(StatusCodes.Status409Conflict, result);
        }

        [TestMethod]
        public async Task EndpointOperationAsync_NotFoundError()
        {
            // arrange
            var c = new TestController();

            // act
            var result = await c.NotFoundErrorAsync();

            // assert
            Assert.IsNotNull(result);
            ObjectResultValidation.AssertStatusCode(StatusCodes.Status404NotFound, result);
        }
    }
}
