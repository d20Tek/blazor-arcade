//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;

namespace Blazor.Arcade.Client.UnitTests.Services
{
    [TestClass]
    public class MessageBoxServiceTests
    {
        private readonly Mock<IJSRuntime> _jsRuntime = new Mock<IJSRuntime>();

        [TestMethod]
        public async Task Alert()
        {
            // arrange
            var msgbox = new MessageBoxService(_jsRuntime.Object);

            // act
            await msgbox.Alert("test alert");

            // assert
            _jsRuntime.Verify(o => o.InvokeAsync<IJSVoidResult>("alert", new[] { "test alert" }), Times.Once());
        }

        [TestMethod]
        public async Task Confirm_True()
        {
            // arrange
            _jsRuntime.Setup(x => x.InvokeAsync<bool>("confirm", new[] { "test confirm?" }))
                      .ReturnsAsync(true);
            var msgbox = new MessageBoxService(_jsRuntime.Object);

            // act
            var result = await msgbox.Confirm("test confirm?");

            // assert
            Assert.IsTrue(result);
            _jsRuntime.Verify(o => o.InvokeAsync<bool>("confirm", new[] { "test confirm?" }), Times.Once());
        }


        [TestMethod]
        public async Task Confirm_False()
        {
            // arrange
            var msgbox = new MessageBoxService(_jsRuntime.Object);

            // act
            var result = await msgbox.Confirm("test confirm?");

            // assert
            Assert.IsFalse(result);
            _jsRuntime.Verify(o => o.InvokeAsync<bool>("confirm", new[] { "test confirm?" }), Times.Once());
        }
    }
}
