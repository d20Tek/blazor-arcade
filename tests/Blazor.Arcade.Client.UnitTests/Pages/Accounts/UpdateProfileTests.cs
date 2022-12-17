//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Pages.Profiles;
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Client.UnitTests.Mocks;
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
    public class UpdateProfileTests
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
            var comp = ctx.RenderComponent<UpdateProfile>(parameters => parameters
                .Add(p => p.ProfileId, "test-profile-1"));

            // assert
            var expectedHtml =
@"
<h4>Change Profile</h4>
<p>Change this profile's information that is displayed in Arcade.</p>
<hr>
<div class=""row"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <form id=""profile-form"" >
      <div class=""mb-2"">There is no profile found for id: test-profile-1.</div>
      <input type=""submit"" id=""submit-btn"" class=""btn btn-primary"" value=""Save"">
      <a class=""btn btn-light"" href=""/profiles/manage"">Cancel</a>
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
            var account = new UserProfile
            {
                Id = "test-profile-1",
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
            var comp = ctx.RenderComponent<UpdateProfile>(parameters => parameters
                .Add(p => p.ProfileId, "test-profile-1"));

            // assert
            var expectedHtml =
@"
<h4>Change Profile</h4>
<p>Change this profile's information that is displayed in Arcade.</p>
<hr>
<div class=""row"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <form id=""profile-form"" >
      <div class=""form-group"">
        <label>Profile Id:</label>
        <div>test-profile-1</div>
      </div>
      <div class=""form-group mt-2"">
        <label>Logged In User:</label>
        <div>test-user-1</div>
      </div>
      <div class=""form-group mt-2"">
        <label for=""profile-name"">Name:</label>
        <input id=""profile-name"" placeholder=""Enter display name..."" 
               class=""form-control valid"" value=""Test1""  >
      </div>
      <div class=""form-group mt-2"">
        <label for=""profile-server"">Select Server:</label>
        <select id=""profile-server"" class=""form-control valid"" value=""s1""  >
          <option value=""s1"">s1</option>
        </select>
      </div>
      <div class=""form-group mt-2"">
        <label for=""profile-gender"">Gender:</label>
        <select id=""profile-gender"" class=""form-control valid"" value=""U""  >
          <option value=""M"">Male</option>
          <option value=""F"">Female</option>
          <option value=""U"">Unspecified</option>
        </select>
      </div>
      <div class=""form-group my-2"">
        <label for=""profile-avatar"">Avatar (coming soon):</label>
        <input class=""form-control"" id=""profile-avatar"" disabled=""""
               value="""" type=""file"" >
      </div>
      <input type=""submit"" id=""submit-btn"" class=""btn btn-primary"" value=""Save"">
      <a class=""btn btn-light"" href=""/profiles/manage"">Cancel</a>
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
            var account = new UserProfile
            {
                Id = "test-profile-1",
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

            var comp = ctx.RenderComponent<UpdateProfile>(parameters => parameters
                .Add(p => p.ProfileId, "test-profile-1"));

            // act
            comp.Find("#profile-name").Change("Test User");
            comp.Find("#submit-btn").Click();
            // assert
            var expectedHtml =
@"
<h4>Change Profile</h4>
<p>Change this profile's information that is displayed in Arcade.</p>
<hr>
<div class=""row"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <form id=""profile-form"" >
      <div class=""form-group"">
        <label>Profile Id:</label>
        <div>test-profile-1</div>
      </div>
      <div class=""form-group mt-2"">
        <label>Logged In User:</label>
        <div>test-user-1</div>
      </div>
      <div class=""form-group mt-2"">
        <label for=""profile-name"">Name:</label>
        <input id=""profile-name"" placeholder=""Enter display name..."" 
               class=""form-control modified valid"" value=""Test User""  >
      </div>
      <div class=""form-group mt-2"">
        <label for=""profile-server"">Select Server:</label>
        <select id=""profile-server"" class=""form-control valid"" value=""s1""  >
          <option value=""s1"">s1</option>
        </select>
      </div>
      <div class=""form-group mt-2"">
        <label for=""profile-gender"">Gender:</label>
        <select id=""profile-gender"" class=""form-control valid"" value=""U""  >
          <option value=""M"">Male</option>
          <option value=""F"">Female</option>
          <option value=""U"">Unspecified</option>
        </select>
      </div>
      <div class=""form-group my-2"">
        <label for=""profile-avatar"">Avatar (coming soon):</label>
        <input class=""form-control"" id=""profile-avatar"" disabled="""" value="""" type=""file"" >
      </div>
      <input type=""submit"" id=""submit-btn"" class=""btn btn-primary"" value=""Save"">
      <a class=""btn btn-light"" href=""/profiles/manage"">Cancel</a>
    </form>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
            StringAssert.Contains(_mockNav.Uri, "/profiles/manage");
        }

        [TestMethod]
        public void FormSubmitted_NotValid()
        {
            // arrange
            var account = new UserProfile
            {
                Id = "test-profile-1",
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

            var comp = ctx.RenderComponent<UpdateProfile>(parameters => parameters
                .Add(p => p.ProfileId, "test-profile-1"));

            // act
            comp.Find("#profile-name").Change("");
            comp.Find("#submit-btn").Click();
            // assert
            var expectedHtml =
@"
<h4>Change Profile</h4>
<p>Change this profile's information that is displayed in Arcade.</p>
<hr>
<div class=""row"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <form id=""profile-form"" >
      <div class=""form-group"">
        <label>Profile Id:</label>
        <div>test-profile-1</div>
      </div>
      <div class=""form-group mt-2"">
        <label>Logged In User:</label>
        <div>test-user-1</div>
      </div>
      <div class=""form-group mt-2"">
        <label for=""profile-name"">Name:</label>
        <input id=""profile-name"" placeholder=""Enter display name..."" aria-invalid=""true""
               class=""form-control modified invalid"" value=""""  >
        <div class=""validation-message"">The Name field is required.</div>
      </div>
      <div class=""form-group mt-2"">
        <label for=""profile-server"">Select Server:</label>
        <select id=""profile-server"" class=""form-control valid"" value=""s1""  >
          <option value=""s1"">s1</option>
        </select>
      </div>
      <div class=""form-group mt-2"">
        <label for=""profile-gender"">Gender:</label>
        <select id=""profile-gender"" class=""form-control valid"" value=""U""  >
          <option value=""M"">Male</option>
          <option value=""F"">Female</option>
          <option value=""U"">Unspecified</option>
        </select>
      </div>
      <div class=""form-group my-2"">
        <label for=""profile-avatar"">Avatar (coming soon):</label>
        <input class=""form-control"" id=""profile-avatar"" disabled="""" value="""" type=""file"" >
      </div>
      <input type=""submit"" id=""submit-btn"" class=""btn btn-primary"" value=""Save"">
      <a class=""btn btn-light"" href=""/profiles/manage"">Cancel</a>
    </form>
  </div>
</div>";
            comp.MarkupMatches(expectedHtml);
            Assert.IsFalse(_mockNav.Uri.Contains("/profiles/manage"));
        }
    }
}
