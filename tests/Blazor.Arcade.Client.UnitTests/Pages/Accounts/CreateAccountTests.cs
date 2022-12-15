//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Pages.Accounts;
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
    public class CreateAccountTests
    {
        private readonly NavigationManager _mockNav = new MockNavigationManager();

        [TestMethod]
        public void Render_Initial()
        {
            // arrange
            var _storage = new Mock<ILocalStorageService>().Object;
            var _accountServ = new Mock<ICrudClientService<UserAccount>>();

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<ILocalStorageService>(_storage);
            ctx.Services.AddSingleton<ICrudClientService<UserAccount>>(_accountServ.Object);
            ctx.Services.AddSingleton<NavigationManager>(_mockNav);

            // act
            var comp = ctx.RenderComponent<CreateAccount>();

            // assert
            var expectedHtml =
@"
<h4>New Account</h4>
<p>Create a new account to use in Arcade. This will show you as a different player
with a new display name. And allow you to create accounts on different servers.</p>
<hr>
<div class=""row"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <form id=""account-form"" >
      <div class=""form-group"">
        <label for=""account-name"">Name:</label>
        <input id=""account-name"" placeholder=""Enter display name...""
               class=""form-control valid"" value=""""  >
      </div>
      <div class=""form-group mt-2"">
        <label for=""account-server"">Select Server:</label>
        <select id=""account-server"" class=""form-control valid"" value=""""  >
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
        <input class=""form-control"" id=""account-avatar"" disabled="""" value="""" type=""file"" >
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
            var _storage = new Mock<ILocalStorageService>();
            var _accountServ = new Mock<ICrudClientService<UserAccount>>();

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<ILocalStorageService>(_storage.Object);
            ctx.Services.AddSingleton<ICrudClientService<UserAccount>>(_accountServ.Object);
            ctx.Services.AddSingleton<NavigationManager>(_mockNav);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            var comp = ctx.RenderComponent<CreateAccount>();

            // act
            comp.Find("#account-name").Change("Test User");
            comp.Find("#account-server").Change("s1");
            comp.Find("#account-gender").Change("M");
            comp.Find("#submit-btn").Click();

            // assert
            var expectedHtml =
@"
<h4>New Account</h4>
<p>Create a new account to use in Arcade. This will show you as a different player 
with a new display name. And allow you to create accounts on different servers.</p>
<hr>
<div class=""row"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <form id=""account-form"" >
      <div class=""form-group"">
        <label for=""account-name"">Name:</label>
        <input id=""account-name"" placeholder=""Enter display name..."" 
               class=""form-control modified valid"" value=""Test User""  >
      </div>
      <div class=""form-group mt-2"">
        <label for=""account-server"">Select Server:</label>
        <select id=""account-server"" class=""form-control modified valid"" value=""s1""  >
          <option value=""s1"">s1</option>
        </select>
      </div>
      <div class=""form-group mt-2"">
        <label for=""account-gender"">Gender:</label>
        <select id=""account-gender"" class=""form-control modified valid"" value=""M""  >
          <option value=""M"">Male</option>
          <option value=""F"">Female</option>
          <option value=""U"">Unspecified</option>
        </select>
      </div>
      <div class=""form-group my-2"">
        <label for=""account-avatar"">Avatar (coming soon):</label>
        <input class=""form-control"" id=""account-avatar"" disabled="""" value="""" type=""file"" >
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
            var _storage = new Mock<ILocalStorageService>();
            var _accountServ = new Mock<ICrudClientService<UserAccount>>();

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<ILocalStorageService>(_storage.Object);
            ctx.Services.AddSingleton<ICrudClientService<UserAccount>>(_accountServ.Object);
            ctx.Services.AddSingleton<NavigationManager>(_mockNav);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            var comp = ctx.RenderComponent<CreateAccount>();

            // act
            comp.Find("#submit-btn").Click();

            // assert
            var expectedHtml =
@"
<h4>New Account</h4>
<p>Create a new account to use in Arcade. This will show you as a different player 
with a new display name. And allow you to create accounts on different servers.</p>
<hr>
<div class=""row"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <form id=""account-form"" >
      <div class=""form-group"">
        <label for=""account-name"">Name:</label>
        <input id=""account-name"" placeholder=""Enter display name..."" aria-invalid=""true""
               class=""form-control invalid"" value=""""  >
        <div class=""validation-message"">The Name field is required.</div>
      </div>
      <div class=""form-group mt-2"">
        <label for=""account-server"">Select Server:</label>
        <select id=""account-server"" aria-invalid=""true""
                class=""form-control invalid"" value=""""  >
          <option value=""s1"">s1</option>
        </select>
        <div class=""validation-message"">The Server field is required.</div>
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
        <input class=""form-control"" id=""account-avatar"" disabled="""" value="""" type=""file"" >
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
