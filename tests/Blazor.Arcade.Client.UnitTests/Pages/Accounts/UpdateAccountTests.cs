//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Pages.Accounts;
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Client.UnitTests.Mocks;
using Blazor.Arcade.Common.Core.Client;
using Blazor.Arcade.Common.Models;
using Blazored.LocalStorage;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Blazor.Arcade.Client.UnitTests.Pages.Accounts
{
    [TestClass]
    public class UpdateAccountTests
    {
        private readonly NavigationManager _mockNav = new MockNavigationManager();
        private readonly ILocalStorageService _storage = new Mock<ILocalStorageService>().Object;

        [TestMethod]
        public void Render_Empty()
        {
            // arrange
            var _accountServ = new Mock<IUserProfileClientService>();

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<ILocalStorageService>(_storage);
            ctx.Services.AddSingleton<IUserProfileClientService>(_accountServ.Object);
            ctx.Services.AddSingleton<NavigationManager>(_mockNav);

            // act
            var comp = ctx.RenderComponent<UpdateAccount>(parameters => parameters
                .Add(p => p.AccountId, "test-account-1"));

            // assert
            var expectedHtml =
@"
<h4>Change Account</h4>
<p>Change this account's information that is displayed in Arcade.</p>
<hr>
<div class=""row"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <form id=""account-form"" >
      <div class=""mb-2"">There is no account found for id: test-account-1.</div>
      <input type=""submit"" id=""submit-btn"" class=""btn btn-primary"" value=""Save"">
      <a class=""btn btn-light"" href=""/accounts/manage"">Cancel</a>
    </form>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_Initial()
        {
            // arrange
            var account = new UserAccount
            {
                Id = "test-account-1",
                Server = "s1",
                Name = "Test1",
                UserId = "test-user-1"
            };
            var _accountServ = new Mock<IUserProfileClientService>();
            _accountServ.Setup(x => x.GetEntityAsync(It.IsAny<string>()))
                        .ReturnsAsync(account);

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<ILocalStorageService>(_storage);
            ctx.Services.AddSingleton<IUserProfileClientService>(_accountServ.Object);
            ctx.Services.AddSingleton<NavigationManager>(_mockNav);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            // act
            var comp = ctx.RenderComponent<UpdateAccount>(parameters => parameters
                .Add(p => p.AccountId, "test-account-1"));

            // assert
            var expectedHtml =
@"
<h4>Change Account</h4>
<p>Change this account's information that is displayed in Arcade.</p>
<hr>
<div class=""row"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <form id=""account-form"" >
      <div class=""form-group"">
        <label>Account Id:</label>
        <div>test-account-1</div>
      </div>
      <div class=""form-group mt-2"">
        <label>Logged In User:</label>
        <div>test-user-1</div>
      </div>
      <div class=""form-group mt-2"">
        <label for=""account-name"">Name:</label>
        <input id=""account-name"" placeholder=""Enter display name..."" 
               class=""form-control valid"" value=""Test1""  >
      </div>
      <div class=""form-group mt-2"">
        <label for=""account-server"">Select Server:</label>
        <select id=""account-server"" class=""form-control valid"" value=""s1""  >
          <option value=""s1"">s1</option>
        </select>
      </div>
      <div class=""form-group mt-2"">
        <label for=""account-gender"">Gender:</label>
        <select id=""account-gender"" class=""form-control valid"" value=""U""  >
          <option value=""M"">Male</option>
          <option value=""F"">Female</option>
          <option value=""U"">Unspecified</option>
        </select>
      </div>
      <div class=""form-group my-2"">
        <label for=""account-avatar"">Avatar (coming soon):</label>
        <input class=""form-control"" id=""account-avatar"" disabled=""""
               value="""" type=""file"" >
      </div>
      <input type=""submit"" id=""submit-btn"" class=""btn btn-primary"" value=""Save"">
      <a class=""btn btn-light"" href=""/accounts/manage"">Cancel</a>
    </form>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void FormSubmitted()
        {
            // arrange
            var account = new UserAccount
            {
                Id = "test-account-1",
                Server = "s1",
                Name = "Test1",
                UserId = "test-user-1"
            };
            var _accountServ = new Mock<IUserProfileClientService>();
            _accountServ.Setup(x => x.GetEntityAsync(It.IsAny<string>()))
                        .ReturnsAsync(account);

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<ILocalStorageService>(_storage);
            ctx.Services.AddSingleton<IUserProfileClientService>(_accountServ.Object);
            ctx.Services.AddSingleton<NavigationManager>(_mockNav);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            var comp = ctx.RenderComponent<UpdateAccount>(parameters => parameters
                .Add(p => p.AccountId, "test-account-1"));

            // act
            comp.Find("#account-name").Change("Test User");
            comp.Find("#submit-btn").Click();
            // assert
            var expectedHtml =
@"
<h4>Change Account</h4>
<p>Change this account's information that is displayed in Arcade.</p>
<hr>
<div class=""row"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <form id=""account-form"" >
      <div class=""form-group"">
        <label>Account Id:</label>
        <div>test-account-1</div>
      </div>
      <div class=""form-group mt-2"">
        <label>Logged In User:</label>
        <div>test-user-1</div>
      </div>
      <div class=""form-group mt-2"">
        <label for=""account-name"">Name:</label>
        <input id=""account-name"" placeholder=""Enter display name..."" 
               class=""form-control modified valid"" value=""Test User""  >
      </div>
      <div class=""form-group mt-2"">
        <label for=""account-server"">Select Server:</label>
        <select id=""account-server"" class=""form-control valid"" value=""s1""  >
          <option value=""s1"">s1</option>
        </select>
      </div>
      <div class=""form-group mt-2"">
        <label for=""account-gender"">Gender:</label>
        <select id=""account-gender"" class=""form-control valid"" value=""U""  >
          <option value=""M"">Male</option>
          <option value=""F"">Female</option>
          <option value=""U"">Unspecified</option>
        </select>
      </div>
      <div class=""form-group my-2"">
        <label for=""account-avatar"">Avatar (coming soon):</label>
        <input class=""form-control"" id=""account-avatar"" disabled=""""
               value="""" type=""file"" >
      </div>
      <input type=""submit"" id=""submit-btn"" class=""btn btn-primary"" value=""Save"">
      <a class=""btn btn-light"" href=""/accounts/manage"">Cancel</a>
    </form>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
            StringAssert.Contains(_mockNav.Uri, "/accounts/manage");
        }

        [TestMethod]
        public void FormSubmitted_NotValid()
        {
            // arrange
            var account = new UserAccount
            {
                Id = "test-account-1",
                Server = "s1",
                Name = "Test1",
                UserId = "test-user-1"
            };
            var _accountServ = new Mock<IUserProfileClientService>();
            _accountServ.Setup(x => x.GetEntityAsync(It.IsAny<string>()))
                        .ReturnsAsync(account);

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<ILocalStorageService>(_storage);
            ctx.Services.AddSingleton<IUserProfileClientService>(_accountServ.Object);
            ctx.Services.AddSingleton<NavigationManager>(_mockNav);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            var comp = ctx.RenderComponent<UpdateAccount>(parameters => parameters
                .Add(p => p.AccountId, "test-account-1"));

            // act
            comp.Find("#account-name").Change("");
            comp.Find("#submit-btn").Click();
            // assert
            var expectedHtml =
@"
<h4>Change Account</h4>
<p>Change this account's information that is displayed in Arcade.</p>
<hr>
<div class=""row"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <form id=""account-form"" >
      <div class=""form-group"">
        <label>Account Id:</label>
        <div>test-account-1</div>
      </div>
      <div class=""form-group mt-2"">
        <label>Logged In User:</label>
        <div>test-user-1</div>
      </div>
      <div class=""form-group mt-2"">
        <label for=""account-name"">Name:</label>
        <input id=""account-name"" placeholder=""Enter display name..."" aria-invalid=""true""
               class=""form-control modified invalid"" value=""""  >
        <div class=""validation-message"">The Name field is required.</div>
      </div>
      <div class=""form-group mt-2"">
        <label for=""account-server"">Select Server:</label>
        <select id=""account-server"" class=""form-control valid"" value=""s1""  >
          <option value=""s1"">s1</option>
        </select>
      </div>
      <div class=""form-group mt-2"">
        <label for=""account-gender"">Gender:</label>
        <select id=""account-gender"" class=""form-control valid"" value=""U""  >
          <option value=""M"">Male</option>
          <option value=""F"">Female</option>
          <option value=""U"">Unspecified</option>
        </select>
      </div>
      <div class=""form-group my-2"">
        <label for=""account-avatar"">Avatar (coming soon):</label>
        <input class=""form-control"" id=""account-avatar"" disabled=""""
               value="""" type=""file"" >
      </div>
      <input type=""submit"" id=""submit-btn"" class=""btn btn-primary"" value=""Save"">
      <a class=""btn btn-light"" href=""/accounts/manage"">Cancel</a>
    </form>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
            Assert.IsFalse(_mockNav.Uri.Contains("/accounts/manage"));
        }
    }
}
