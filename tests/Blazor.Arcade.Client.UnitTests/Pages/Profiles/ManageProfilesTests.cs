﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Pages.Profiles;
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Common.Models;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Blazor.Arcade.Client.UnitTests.Pages.Accounts
{
    [TestClass]
    public class ManageProfilesTests
    {
        private readonly Mock<IMessageBoxService> _msgService = new Mock<IMessageBoxService>();

        [TestMethod]
        public void Render_Empty()
        {
            // arrange
            var _profileServ = new Mock<IUserProfileClientService>();

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IUserProfileClientService>(_profileServ.Object);
            ctx.Services.AddSingleton<IMessageBoxService>(_msgService.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            // act
            var comp = ctx.RenderComponent<ManageProfiles>();

            // assert
            var expectedHtml =
@"
<div class=""row justify-content-center"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <h4>Manage Profiles</h4>
    <div style=""font-weight: bold"">Current Profile:</div>
    <div>You currently don't have an Arcade profile selected, 
         please create a new profile or select one from the list below.</div>
    <hr>
    <div >Switch Profile:</div>
    <div >You don't have any profiles set up yet. Create a player profile.</div>
    <a id=""create-profile-btn"" class=""btn btn-outline-light"" href=""/profile/create"">
      Create New Profile
    </a>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
            Assert.AreEqual("/profile/update/", comp.Instance.EditProfileUrl);
        }

        [TestMethod]
        public void Render()
        {
            // arrange
            var _currentprofile = new UserProfile { Id = "test-profile-1", Name = "Test1", Server = "s1", UserId = "test-user-1" };
            var _profileList = new List<UserProfile>
            {
                new UserProfile { Id = "test-profile-1", Name = "Test1", Server = "s1", UserId = "test-user-1" },
                new UserProfile { Id = "test-profile-2", Name = "Test2", Server = "s2", UserId = "test-user-1" },
                new UserProfile { Id = "test-profile-3", Name = "Test3", Server = "s3", UserId = "test-user-1" }
            };

            var _profileServ = new Mock<IUserProfileClientService>();
            _profileServ.Setup(x => x.GetEntitiesAsync()).ReturnsAsync(_profileList);
            _profileServ.Setup(x => x.GetCurrentProfileAsync(It.IsAny<Task<AuthenticationState>?>()))
                        .ReturnsAsync(_currentprofile);

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IUserProfileClientService>(_profileServ.Object);
            ctx.Services.AddSingleton<IMessageBoxService>(_msgService.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            // act
            var comp = ctx.RenderComponent<ManageProfiles>();

            // assert
            var expectedHtml =
@"
<div class=""row justify-content-center"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <h4>Manage Profiles</h4>
    <div style=""font-weight: bold"">Current Profile:</div>
    <div class=""float-end"">
      <div>Avatar:</div>
      <img class=""border"" width=""64"" src=""/images/coming-soon.png"">
    </div>
    <div>Id: test-profile-1</div>
    <div>Name: Test1</div>
    <div>Server: s1</div>
    <div>Gender: U</div>
    <div class=""mt-2"">
      <a id=""edit-profile-btn"" class=""btn btn-outline-light"" href=""/profile/update/test-profile-1"">
        Edit Profile Info
      </a>
      <button id=""delete-profile-btn"" class=""btn btn-outline-light"" >
        Delete This Profile
      </button>
    </div>
    <hr>
    <div >Switch Profile:</div>
    <table class=""table table-hover table-sm"" >
      <thead >
        <tr >
          <th scope=""col"" class=""glyph-column"" ></th>
          <th scope=""col"" >Name</th>
          <th scope=""col"" class=""text-end"" >Server</th>
        </tr>
      </thead>
      <tbody >
        <tr id=""test-profile-1""  >
          <td >
            <span class=""oi oi-check glyph-mark"" ></span>
          </td>
          <td >Test1</td>
          <td class=""text-end"" >s1</td>
        </tr>
        <tr id=""test-profile-2""  >
          <td ></td>
          <td >Test2</td>
          <td class=""text-end"" >s2</td>
        </tr>
        <tr id=""test-profile-3""  >
          <td ></td>
          <td >Test3</td>
          <td class=""text-end"" >s3</td>
        </tr>
      </tbody>
      <tfoot >
        <tr >
          <td class=""py-2"" colspan=""3"" >
            <button id=""switch-profile-btn"" class=""btn btn-primary"" disabled=""""  >
              Switch Profile
            </button>
          </td>
        </tr>
      </tfoot>
    </table>
    <a id=""create-profile-btn"" class=""btn btn-outline-light"" href=""/profile/create"">
      Create New Profile
    </a>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_WithNoCachedProfile()
        {
            // arrange
            var _profileList = new List<UserProfile>
            {
                new UserProfile { Id = "test-profile-1", Name = "Test1", Server = "s1", UserId = "test-user-1" },
                new UserProfile { Id = "test-profile-2", Name = "Test2", Server = "s2", UserId = "test-user-1" },
                new UserProfile { Id = "test-profile-3", Name = "Test3", Server = "s3", UserId = "test-user-1" }
            };

            var _profileServ = new Mock<IUserProfileClientService>();
            _profileServ.Setup(x => x.GetEntitiesAsync()).ReturnsAsync(_profileList);

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IUserProfileClientService>(_profileServ.Object);
            ctx.Services.AddSingleton<IMessageBoxService>(_msgService.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            // act
            var comp = ctx.RenderComponent<ManageProfiles>();

            // assert
            var expectedHtml =
@"
<div class=""row justify-content-center"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <h4>Manage Profiles</h4>
    <div style=""font-weight: bold"">Current Profile:</div>
    <div>You currently don't have an Arcade profile selected, 
         please create a new profile or select one from the list below.</div>
    <hr>
    <div >Switch Profile:</div>
    <table class=""table table-hover table-sm"" >
      <thead >
        <tr >
          <th scope=""col"" class=""glyph-column"" ></th>
          <th scope=""col"" >Name</th>
          <th scope=""col"" class=""text-end"" >Server</th>
        </tr>
      </thead>
      <tbody >
        <tr id=""test-profile-1""  >
          <td ></td>
          <td >Test1</td>
          <td class=""text-end"" >s1</td>
        </tr>
        <tr id=""test-profile-2""  >
          <td ></td>
          <td >Test2</td>
          <td class=""text-end"" >s2</td>
        </tr>
        <tr id=""test-profile-3""  >
          <td ></td>
          <td >Test3</td>
          <td class=""text-end"" >s3</td>
        </tr>
      </tbody>
      <tfoot >
        <tr >
          <td class=""py-2"" colspan=""3"" >
            <button id=""switch-profile-btn"" class=""btn btn-primary"" disabled=""""  >
              Switch Profile
            </button>
          </td>
        </tr>
      </tfoot>
    </table>
    <a id=""create-profile-btn"" class=""btn btn-outline-light"" href=""/profile/create"">
      Create New Profile
    </a>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_DeleteProfile()
        {
            // arrange
            var _profileList = new List<UserProfile>
            {
                new UserProfile { Id = "test-profile-1", Name = "Test1", Server = "s1", UserId = "test-user-1" },
                new UserProfile { Id = "test-profile-2", Name = "Test2", Server = "s1", UserId = "test-user-1" },
                new UserProfile { Id = "test-profile-3", Name = "Test3", Server = "s3", UserId = "test-user-1" }
            };

            var _profileServ = new Mock<IUserProfileClientService>();
            _profileServ.Setup(x => x.GetEntitiesAsync()).ReturnsAsync(_profileList);

            _msgService.Setup(x => x.Confirm(It.IsAny<string>())).ReturnsAsync(true);

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IUserProfileClientService>(_profileServ.Object);
            ctx.Services.AddSingleton<IMessageBoxService>(_msgService.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            var comp = ctx.RenderComponent<ManageProfiles>();

            // act
            comp.Find("#test-profile-2").Click();
            comp.Find("#switch-profile-btn").Click();
            _profileList.RemoveAt(1);
            comp.Find("#delete-profile-btn").Click();

            // assert
            var expectedHtml =
@"
<div class=""row justify-content-center"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <h4>Manage Profiles</h4>
    <div style=""font-weight: bold"">Current Profile:</div>
    <div>You currently don't have an Arcade profile selected, 
         please create a new profile or select one from the list below.</div>
    <hr>
    <div >Switch Profile:</div>
    <table class=""table table-hover table-sm"" >
      <thead >
        <tr >
          <th scope=""col"" class=""glyph-column"" ></th>
          <th scope=""col"" >Name</th>
          <th scope=""col"" class=""text-end"" >Server</th>
        </tr>
      </thead>
      <tbody >
        <tr id=""test-profile-1""  >
          <td ></td>
          <td >Test1</td>
          <td class=""text-end"" >s1</td>
        </tr>
        <tr id=""test-profile-3""  >
          <td ></td>
          <td >Test3</td>
          <td class=""text-end"" >s3</td>
        </tr>
      </tbody>
      <tfoot >
        <tr >
          <td class=""py-2"" colspan=""3"" >
            <button id=""switch-profile-btn"" class=""btn btn-primary""  >
              Switch Profile
            </button>
          </td>
        </tr>
      </tfoot>
    </table>
    <a id=""create-profile-btn"" class=""btn btn-outline-light"" href=""/profile/create"">
      Create New Profile
    </a>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_DeleteProfile_CancelConfirm()
        {
            // arrange
            var _profileList = new List<UserProfile>
            {
                new UserProfile { Id = "test-profile-1", Name = "Test1", Server = "s1", UserId = "test-user-1" },
                new UserProfile { Id = "test-profile-2", Name = "Test2", Server = "s1", UserId = "test-user-1" },
                new UserProfile { Id = "test-profile-3", Name = "Test3", Server = "s3", UserId = "test-user-1" }
            };

            var _profileServ = new Mock<IUserProfileClientService>();
            _profileServ.Setup(x => x.GetEntitiesAsync()).ReturnsAsync(_profileList);

            _msgService.Setup(x => x.Confirm(It.IsAny<string>())).ReturnsAsync(false);

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IUserProfileClientService>(_profileServ.Object);
            ctx.Services.AddSingleton<IMessageBoxService>(_msgService.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            var comp = ctx.RenderComponent<ManageProfiles>();

            // act
            comp.Find("#test-profile-2").Click();
            comp.Find("#switch-profile-btn").Click();
            comp.Find("#delete-profile-btn").Click();

            // assert
            var expectedHtml =
@"
<div class=""row justify-content-center"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <h4>Manage Profiles</h4>
    <div style=""font-weight: bold"">Current Profile:</div>
    <div class=""float-end"">
      <div>Avatar:</div>
      <img class=""border"" width=""64"" src=""/images/coming-soon.png"">
    </div>
    <div>Id: test-profile-2</div>
    <div>Name: Test2</div>
    <div>Server: s1</div>
    <div>Gender: U</div>
    <div class=""mt-2"">
      <a id=""edit-profile-btn"" class=""btn btn-outline-light"" href=""/profile/update/test-profile-2"">
        Edit Profile Info
      </a>
      <button id=""delete-profile-btn"" class=""btn btn-outline-light"" >
        Delete This Profile
      </button>
    </div>
    <hr>
    <div >Switch Profile:</div>
    <table class=""table table-hover table-sm"" >
      <thead >
        <tr >
          <th scope=""col"" class=""glyph-column"" ></th>
          <th scope=""col"" >Name</th>
          <th scope=""col"" class=""text-end"" >Server</th>
        </tr>
      </thead>
      <tbody >
        <tr id=""test-profile-1""  >
          <td ></td>
          <td >Test1</td>
          <td class=""text-end"" >s1</td>
        </tr>
        <tr id=""test-profile-2"" class=""selected""  >
          <td >
            <span class=""oi oi-check glyph-mark"" ></span>
          </td>
          <td >Test2</td>
          <td class=""text-end"" >s1</td>
        </tr>
        <tr id=""test-profile-3""  >
          <td ></td>
          <td >Test3</td>
          <td class=""text-end"" >s3</td>
        </tr>
      </tbody>
      <tfoot >
        <tr >
          <td class=""py-2"" colspan=""3"" >
            <button id=""switch-profile-btn"" class=""btn btn-primary""  >
              Switch Profile
            </button>
          </td>
        </tr>
      </tfoot>
    </table>
    <a id=""create-profile-btn"" class=""btn btn-outline-light"" href=""/profile/create"">
      Create New Profile
    </a>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }
    }
}
