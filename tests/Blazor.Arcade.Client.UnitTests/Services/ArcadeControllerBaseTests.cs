//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Client;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Arcade.Client.UnitTests.Services
{
    [TestClass]
    public class ArcadeControllerBaseTests
    {
        public sealed class TestService : ClientServiceBase
        {
            public TestService()
                : base(new Mock<ILogger<TestService>>().Object)
            {
            }

            public async Task<bool> SucceedAsync()
            {
                return await ServiceOperationAsync<bool>("Succeed", () =>
                {
                    return Task.FromResult<bool>(true);
                });
            }

            public async Task<int> SucceedAfterRetryAsync()
            {
                var result = 0;
                return await ServiceOperationAsync<int>("SucceedAfterRetry", () =>
                {
                    result++;
                    if (result <= 1)
                    {
                        throw new InvalidOperationException();
                    }

                    return Task.FromResult<int>(result);
                });
            }

            [ExcludeFromCodeCoverage]
            public async Task<bool> ErrorAfterRetriesAsync()
            {
                return await ServiceOperationAsync<bool>("ErrorAfterRetries", () =>
                {
                    throw new InvalidOperationException();
                });
            }
        }

        [TestMethod]
        public async Task ServiceOperationAsync_Succeed()
        {
            // arrange
            var service = new TestService();

            // act
            var result = await service.SucceedAsync();

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ServiceOperationAsync_SucceedAfterRetry()
        {
            // arrange
            var service = new TestService();

            // act
            var result = await service.SucceedAfterRetryAsync();

            // assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [ExcludeFromCodeCoverage]
        public async Task ServiceOperationAsync_ErrorAfterRetries()
        {
            // arrange
            var service = new TestService();

            // act
            _ = await service.ErrorAfterRetriesAsync();
        }
    }
}
