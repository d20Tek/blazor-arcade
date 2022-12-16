//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Pages.Accounts;
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Common.Core.Client;
using Blazor.Arcade.Common.Models;
using Blazored.LocalStorage;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Blazor.Arcade.Client.UnitTests.Pages.Accounts
{
    [TestClass]
    public class ManageAccountsTests
    {
        private readonly Mock<IMessageBoxService> _msgService = new Mock<IMessageBoxService>();

        [TestMethod]
        public void Render_Empty()
        {
            // arrange
            var _storage = new Mock<ILocalStorageService>().Object;
            var _accountServ = new Mock<IUserProfileClientService>();

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<ILocalStorageService>(_storage);
            ctx.Services.AddSingleton<IUserProfileClientService>(_accountServ.Object);
            ctx.Services.AddSingleton<IMessageBoxService>(_msgService.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            // act
            var comp = ctx.RenderComponent<ManageAccounts>();

            // assert
            var expectedHtml =
@"
<div class=""row justify-content-center"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <h4>Manage Accounts</h4>
    <div style=""font-weight: bold"">Current Account:</div>
    <div>You currently don't have an Arcade profile selected, 
         please create a new profile or select one from the list below.</div>
    <hr>
    <div >Switch Account:</div>
    <div >You don't have any accounts set up yet. Create a player account.</div>
    <a id=""create-account-btn"" class=""btn btn-outline-light"" href=""/account/create"">
      Create New Account
    </a>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
            Assert.AreEqual("/account/update/", comp.Instance.EditAccountUrl);
        }

        [TestMethod]
        public void Render()
        {
            // arrange
            var _currentAccount = new UserAccount { Id = "test-account-1", Name = "Test1", Server = "s1", UserId = "test-user-1" };
            var _accountList = new List<UserAccount>
            {
                new UserAccount { Id = "test-account-1", Name = "Test1", Server = "s1", UserId = "test-user-1" },
                new UserAccount { Id = "test-account-2", Name = "Test2", Server = "s2", UserId = "test-user-1" },
                new UserAccount { Id = "test-account-3", Name = "Test3", Server = "s3", UserId = "test-user-1" }
            };

        var _storage = new Mock<ILocalStorageService>();
            _storage.Setup(x => x.GetItemAsync<UserAccount>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_currentAccount);
            var _accountServ = new Mock<IUserProfileClientService>();
            _accountServ.Setup(x => x.GetEntitiesAsync()).ReturnsAsync(_accountList);

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<ILocalStorageService>(_storage.Object);
            ctx.Services.AddSingleton<IUserProfileClientService>(_accountServ.Object);
            ctx.Services.AddSingleton<IMessageBoxService>(_msgService.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            // act
            var comp = ctx.RenderComponent<ManageAccounts>();

            // assert
            var expectedHtml =
@"
<div class=""row justify-content-center"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <h4>Manage Accounts</h4>
    <div style=""font-weight: bold"">Current Account:</div>
    <div class=""float-end"">
      <div>Avatar:</div>
      <img class=""border"" width=""64"" src=""/images/coming-soon.png"">
    </div>
    <div>Id: test-account-1</div>
    <div>Name: Test1</div>
    <div>Server: s1</div>
    <div>Gender: U</div>
    <div class=""mt-2"">
      <a id=""edit-account-btn"" class=""btn btn-outline-light"" href=""/account/update/test-account-1"">
        Edit Account Info
      </a>
      <button id=""delete-account-btn"" class=""btn btn-outline-light"" >
        Delete This Account
      </button>
    </div>
    <hr>
    <div >Switch Account:</div>
    <table class=""table table-hover table-sm"" >
      <thead >
        <tr >
          <th scope=""col"" class=""glyph-column"" ></th>
          <th scope=""col"" >Name</th>
          <th scope=""col"" class=""text-end"" >Server</th>
        </tr>
      </thead>
      <tbody >
        <tr id=""test-account-1""  >
          <td >
            <span class=""oi oi-check glyph-mark"" ></span>
          </td>
          <td >Test1</td>
          <td class=""text-end"" >s1</td>
        </tr>
        <tr id=""test-account-2""  >
          <td ></td>
          <td >Test2</td>
          <td class=""text-end"" >s2</td>
        </tr>
        <tr id=""test-account-3""  >
          <td ></td>
          <td >Test3</td>
          <td class=""text-end"" >s3</td>
        </tr>
      </tbody>
      <tfoot >
        <tr >
          <td class=""py-2"" colspan=""3"" >
            <button id=""switch-account-btn"" class=""btn btn-primary"" disabled=""""  >
              Switch Account
            </button>
          </td>
        </tr>
      </tfoot>
    </table>
    <a id=""create-account-btn"" class=""btn btn-outline-light"" href=""/account/create"">
      Create New Account
    </a>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_WithNoCachedAccount()
        {
            // arrange
            var _accountList = new List<UserAccount>
            {
                new UserAccount { Id = "test-account-1", Name = "Test1", Server = "s1", UserId = "test-user-1" },
                new UserAccount { Id = "test-account-2", Name = "Test2", Server = "s2", UserId = "test-user-1" },
                new UserAccount { Id = "test-account-3", Name = "Test3", Server = "s3", UserId = "test-user-1" }
            };

            var _storage = new Mock<ILocalStorageService>();
            var _accountServ = new Mock<IUserProfileClientService>();
            _accountServ.Setup(x => x.GetEntitiesAsync()).ReturnsAsync(_accountList);

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<ILocalStorageService>(_storage.Object);
            ctx.Services.AddSingleton<IUserProfileClientService>(_accountServ.Object);
            ctx.Services.AddSingleton<IMessageBoxService>(_msgService.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            // act
            var comp = ctx.RenderComponent<ManageAccounts>();

            // assert
            var expectedHtml =
@"
<div class=""row justify-content-center"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <h4>Manage Accounts</h4>
    <div style=""font-weight: bold"">Current Account:</div>
    <div>You currently don't have an Arcade profile selected, 
         please create a new profile or select one from the list below.</div>
    <hr>
    <div >Switch Account:</div>
    <table class=""table table-hover table-sm"" >
      <thead >
        <tr >
          <th scope=""col"" class=""glyph-column"" ></th>
          <th scope=""col"" >Name</th>
          <th scope=""col"" class=""text-end"" >Server</th>
        </tr>
      </thead>
      <tbody >
        <tr id=""test-account-1""  >
          <td ></td>
          <td >Test1</td>
          <td class=""text-end"" >s1</td>
        </tr>
        <tr id=""test-account-2""  >
          <td ></td>
          <td >Test2</td>
          <td class=""text-end"" >s2</td>
        </tr>
        <tr id=""test-account-3""  >
          <td ></td>
          <td >Test3</td>
          <td class=""text-end"" >s3</td>
        </tr>
      </tbody>
      <tfoot >
        <tr >
          <td class=""py-2"" colspan=""3"" >
            <button id=""switch-account-btn"" class=""btn btn-primary"" disabled=""""  >
              Switch Account
            </button>
          </td>
        </tr>
      </tfoot>
    </table>
    <a id=""create-account-btn"" class=""btn btn-outline-light"" href=""/account/create"">
      Create New Account
    </a>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_DeleteAccount()
        {
            // arrange
            var _accountList = new List<UserAccount>
            {
                new UserAccount { Id = "test-account-1", Name = "Test1", Server = "s1", UserId = "test-user-1" },
                new UserAccount { Id = "test-account-2", Name = "Test2", Server = "s1", UserId = "test-user-1" },
                new UserAccount { Id = "test-account-3", Name = "Test3", Server = "s3", UserId = "test-user-1" }
            };

            var _storage = new Mock<ILocalStorageService>();
            var _accountServ = new Mock<IUserProfileClientService>();
            _accountServ.Setup(x => x.GetEntitiesAsync()).ReturnsAsync(_accountList);

            _msgService.Setup(x => x.Confirm(It.IsAny<string>())).ReturnsAsync(true);

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<ILocalStorageService>(_storage.Object);
            ctx.Services.AddSingleton<IUserProfileClientService>(_accountServ.Object);
            ctx.Services.AddSingleton<IMessageBoxService>(_msgService.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            var comp = ctx.RenderComponent<ManageAccounts>();

            // act
            comp.Find("#test-account-2").Click();
            comp.Find("#switch-account-btn").Click();
            _accountList.RemoveAt(1);
            comp.Find("#delete-account-btn").Click();

            // assert
            var expectedHtml =
@"
<div class=""row justify-content-center"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <h4>Manage Accounts</h4>
    <div style=""font-weight: bold"">Current Account:</div>
    <div>You currently don't have an Arcade profile selected, 
         please create a new profile or select one from the list below.</div>
    <hr>
    <div >Switch Account:</div>
    <table class=""table table-hover table-sm"" >
      <thead >
        <tr >
          <th scope=""col"" class=""glyph-column"" ></th>
          <th scope=""col"" >Name</th>
          <th scope=""col"" class=""text-end"" >Server</th>
        </tr>
      </thead>
      <tbody >
        <tr id=""test-account-1""  >
          <td ></td>
          <td >Test1</td>
          <td class=""text-end"" >s1</td>
        </tr>
        <tr id=""test-account-3""  >
          <td ></td>
          <td >Test3</td>
          <td class=""text-end"" >s3</td>
        </tr>
      </tbody>
      <tfoot >
        <tr >
          <td class=""py-2"" colspan=""3"" >
            <button id=""switch-account-btn"" class=""btn btn-primary""  >
              Switch Account
            </button>
          </td>
        </tr>
      </tfoot>
    </table>
    <a id=""create-account-btn"" class=""btn btn-outline-light"" href=""/account/create"">
      Create New Account
    </a>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_DeleteAccount_CancelConfirm()
        {
            // arrange
            var _accountList = new List<UserAccount>
            {
                new UserAccount { Id = "test-account-1", Name = "Test1", Server = "s1", UserId = "test-user-1" },
                new UserAccount { Id = "test-account-2", Name = "Test2", Server = "s1", UserId = "test-user-1" },
                new UserAccount { Id = "test-account-3", Name = "Test3", Server = "s3", UserId = "test-user-1" }
            };

            var _storage = new Mock<ILocalStorageService>();
            var _accountServ = new Mock<IUserProfileClientService>();
            _accountServ.Setup(x => x.GetEntitiesAsync()).ReturnsAsync(_accountList);

            _msgService.Setup(x => x.Confirm(It.IsAny<string>())).ReturnsAsync(false);

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<ILocalStorageService>(_storage.Object);
            ctx.Services.AddSingleton<IUserProfileClientService>(_accountServ.Object);
            ctx.Services.AddSingleton<IMessageBoxService>(_msgService.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            var comp = ctx.RenderComponent<ManageAccounts>();

            // act
            comp.Find("#test-account-2").Click();
            comp.Find("#switch-account-btn").Click();
            comp.Find("#delete-account-btn").Click();

            // assert
            var expectedHtml =
@"
<div class=""row justify-content-center"">
  <div class=""col-12 col-lg-6"" style=""max-width: 600px"">
    <h4>Manage Accounts</h4>
    <div style=""font-weight: bold"">Current Account:</div>
    <div class=""float-end"">
      <div>Avatar:</div>
      <img class=""border"" width=""64"" src=""/images/coming-soon.png"">
    </div>
    <div>Id: test-account-2</div>
    <div>Name: Test2</div>
    <div>Server: s1</div>
    <div>Gender: U</div>
    <div class=""mt-2"">
      <a id=""edit-account-btn"" class=""btn btn-outline-light"" href=""/account/update/test-account-2"">
        Edit Account Info
      </a>
      <button id=""delete-account-btn"" class=""btn btn-outline-light"" >
        Delete This Account
      </button>
    </div>
    <hr>
    <div >Switch Account:</div>
    <table class=""table table-hover table-sm"" >
      <thead >
        <tr >
          <th scope=""col"" class=""glyph-column"" ></th>
          <th scope=""col"" >Name</th>
          <th scope=""col"" class=""text-end"" >Server</th>
        </tr>
      </thead>
      <tbody >
        <tr id=""test-account-1""  >
          <td ></td>
          <td >Test1</td>
          <td class=""text-end"" >s1</td>
        </tr>
        <tr id=""test-account-2"" class=""selected""  >
          <td >
            <span class=""oi oi-check glyph-mark"" ></span>
          </td>
          <td >Test2</td>
          <td class=""text-end"" >s1</td>
        </tr>
        <tr id=""test-account-3""  >
          <td ></td>
          <td >Test3</td>
          <td class=""text-end"" >s3</td>
        </tr>
      </tbody>
      <tfoot >
        <tr >
          <td class=""py-2"" colspan=""3"" >
            <button id=""switch-account-btn"" class=""btn btn-primary""  >
              Switch Account
            </button>
          </td>
        </tr>
      </tfoot>
    </table>
    <a id=""create-account-btn"" class=""btn btn-outline-light"" href=""/account/create"">
      Create New Account
    </a>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }
    }
}
