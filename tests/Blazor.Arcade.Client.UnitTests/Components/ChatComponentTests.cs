//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Components;
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Common.Models;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Blazor.Arcade.Client.UnitTests.Components
{
    [TestClass]
    public class ChatComponentTests
    {
        private readonly Mock<IChatHubClient> _mockChatClient = new Mock<IChatHubClient>();
        private readonly Mock<IUserProfileClientService> _mockProfiles =
            new Mock<IUserProfileClientService>();

        public ChatComponentTests()
        {
            var profile = new UserProfile
            {
                Id = "test-profile-1",
                Name = "Test User",
                UserId = "test-user-1",
                Server = "s1"
            };
            _mockProfiles.Setup(x => x.GetCurrentProfileAsync(It.IsAny<Task<AuthenticationState>>()))
                         .ReturnsAsync(profile);
        }

        [TestMethod]
        public void Render()
        {
            // arrange
            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IChatHubClient>(_mockChatClient.Object);
            ctx.Services.AddSingleton<IUserProfileClientService>(_mockProfiles.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            // act
            var comp = ctx.RenderComponent<ChatComponent>();
            comp.Instance.Dispose();

            // assert
            var expectedHtml =
@"  <h4>Global Chat:</h4>
    <div class=""row"">
      <div class=""col-10 col-md-6"">
        <input type=""text"" id=""message-input"" class=""form-control""
               maxlength=""140"" placeholder=""Enter message..."" value="""" >
      </div>
      <div class=""col-2 col-md-6"">
        <button id=""send-button"" class=""btn btn-primary"" >
          Send
        </button>
      </div>
    </div>
    <div class=""overflow-auto chat-message-list""></div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void SendClicked()
        {
            // arrange
            var message = new ChatMessage
            {
                ProfileId = "test-profile-id",
                ProfileName = "Test User",
                ChannelId = "channel:global",
                MessageId = "message-101",
                Message = "Test message."
            };

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IChatHubClient>(_mockChatClient.Object);
            ctx.Services.AddSingleton<IUserProfileClientService>(_mockProfiles.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            var comp = ctx.RenderComponent<ChatComponent>();

            // act
            comp.Find("#message-input").Change("Test message.");
            comp.Find("#send-button").Click();
            comp.InvokeAsync(() => comp.Instance.OnReceivedMessage(message));

            // assert
            var expectedHtml =
@"  <h4>Global Chat:</h4>
    <div class=""row"">
      <div class=""col-10 col-md-6"">
        <input type=""text"" id=""message-input"" class=""form-control""
               maxlength=""140"" placeholder=""Enter message..."" value="""" >
      </div>
      <div class=""col-2 col-md-6"">
        <button id=""send-button"" class=""btn btn-primary"" >
          Send
        </button>
      </div>
    </div>
    <div class=""overflow-auto chat-message-list"">
      <div>Test User: Test message.</div>
    </div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public async Task SendClicked_NotAuthenticated()
        {
            // arrange
            var profileService = new Mock<IUserProfileClientService>().Object;
            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IUserProfileClientService>(profileService);
            ctx.Services.AddSingleton<IChatHubClient>(_mockChatClient.Object);
            ctx.AddTestAuthorization();

            var comp = ctx.RenderComponent<ChatComponent>();

            // act
            comp.Instance.AuthState = null;
            await comp.Instance.SendClicked();

            // assert
            var expectedHtml =
@"  <div>To get in on the action, you must:
      <a href=""authentication/login"">Log in</a>
    </div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void SendClicked_WithoutOid()
        {
            // arrange
            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IUserProfileClientService>(_mockProfiles.Object);
            ctx.Services.AddSingleton<IChatHubClient>(_mockChatClient.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User");

            var comp = ctx.RenderComponent<ChatComponent>();

            // act
            comp.Find("#message-input").Input("Test message.");
            comp.Find("#send-button").Click();

            // assert
            var expectedHtml =
@"  <h4>Global Chat:</h4>
    <div class=""row"">
      <div class=""col-10 col-md-6"">
        <input type=""text"" id=""message-input"" class=""form-control""
               maxlength=""140"" placeholder=""Enter message..."" value="""" >
      </div>
      <div class=""col-2 col-md-6"">
        <button id=""send-button"" class=""btn btn-primary"" >
          Send
        </button>
      </div>
    </div>
    <div class=""overflow-auto chat-message-list""></div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public async Task OnInputChanged_WithNullEventValue()
        {
            // arrange
            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IUserProfileClientService>(_mockProfiles.Object);
            ctx.Services.AddSingleton<IChatHubClient>(_mockChatClient.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            var comp = ctx.RenderComponent<ChatComponent>();
            var e = new ChangeEventArgs { Value = null };

            // act
            comp.Instance.OnInputChanged(e);
            await comp.Instance.SendClicked();

            // assert
            var expectedHtml =
@"  <h4>Global Chat:</h4>
    <div class=""row"">
      <div class=""col-10 col-md-6"">
        <input type=""text"" id=""message-input"" class=""form-control""
               maxlength=""140"" placeholder=""Enter message..."" value="""" >
      </div>
      <div class=""col-2 col-md-6"">
        <button id=""send-button"" class=""btn btn-primary"" >
          Send
        </button>
      </div>
    </div>
    <div class=""overflow-auto chat-message-list""></div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public async Task OnEnter()
        {
            // arrange
            var message = new ChatMessage
            {
                ProfileId = "test-profile-id",
                ProfileName = "Tester",
                ChannelId = "channel:global",
                MessageId = "message-101",
                Message = "Test message."
            };

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IUserProfileClientService>(_mockProfiles.Object);
            ctx.Services.AddSingleton<IChatHubClient>(_mockChatClient.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            var comp = ctx.RenderComponent<ChatComponent>();
            var e = new KeyboardEventArgs { Code = "Enter" };

            // act
            comp.Find("#message-input").Change("Test message.");
            await comp.Instance.OnEnter(e);
            await comp.InvokeAsync(() => comp.Instance.OnReceivedMessage(message));

            // assert
            var expectedHtml =
@"  <h4>Global Chat:</h4>
    <div class=""row"">
      <div class=""col-10 col-md-6"">
        <input type=""text"" id=""message-input"" class=""form-control""
               maxlength=""140"" placeholder=""Enter message..."" value="""" >
      </div>
      <div class=""col-2 col-md-6"">
        <button id=""send-button"" class=""btn btn-primary"" >
          Send
        </button>
      </div>
    </div>
    <div class=""overflow-auto chat-message-list"">
      <div>Tester: Test message.</div>
    </div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public async Task OnEnter_Numpad()
        {
            // arrange
            var message = new ChatMessage
            {
                ProfileId = "test-profile-id",
                ProfileName = "Tester",
                ChannelId = "channel:global",
                MessageId = "message-101",
                Message = "Test message."
            };

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IUserProfileClientService>(_mockProfiles.Object);
            ctx.Services.AddSingleton<IChatHubClient>(_mockChatClient.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            var comp = ctx.RenderComponent<ChatComponent>();
            var e = new KeyboardEventArgs { Code = "NumpadEnter" };

            // act
            comp.Find("#message-input").Change("Test message.");
            await comp.Instance.OnEnter(e);
            await comp.InvokeAsync(() => comp.Instance.OnReceivedMessage(message));

            // assert
            var expectedHtml =
@"  <h4>Global Chat:</h4>
    <div class=""row"">
      <div class=""col-10 col-md-6"">
        <input type=""text"" id=""message-input"" class=""form-control""
               maxlength=""140"" placeholder=""Enter message..."" value="""" >
      </div>
      <div class=""col-2 col-md-6"">
        <button id=""send-button"" class=""btn btn-primary"" >
          Send
        </button>
      </div>
    </div>
    <div class=""overflow-auto chat-message-list"">
      <div>Tester: Test message.</div>
    </div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public async Task OnEnter_LetterKey()
        {
            // arrange
            var message = new ChatMessage
            {
                ProfileId = "test-profile-id",
                ProfileName = "Tester",
                ChannelId = "channel:global",
                MessageId = "message-101",
                Message = "Test message."
            };

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IUserProfileClientService>(_mockProfiles.Object);
            ctx.Services.AddSingleton<IChatHubClient>(_mockChatClient.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            var comp = ctx.RenderComponent<ChatComponent>();
            var e = new KeyboardEventArgs { Code = "x" };

            // act
            comp.Find("#message-input").Change("Test message.");
            await comp.Instance.OnEnter(e);

            // assert
            var expectedHtml =
@"  <h4>Global Chat:</h4>
    <div class=""row"">
      <div class=""col-10 col-md-6"">
        <input type=""text"" id=""message-input"" class=""form-control""
               maxlength=""140"" placeholder=""Enter message..."" value=""Test message."" >
      </div>
      <div class=""col-2 col-md-6"">
        <button id=""send-button"" class=""btn btn-primary"" >
          Send
        </button>
      </div>
    </div>
    <div class=""overflow-auto chat-message-list""></div>
";
            comp.MarkupMatches(expectedHtml);
        }
    }
}
