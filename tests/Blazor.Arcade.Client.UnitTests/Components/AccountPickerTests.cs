//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Components;
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Common.Models;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Blazor.Arcade.Client.UnitTests.Components
{
    [TestClass]
    public class AccountPickerTests
    {
        private static readonly UserAccount _account = new UserAccount
        {
            Id = "test-account-2",
            Name = "Test2",
            Server = "s1",
            UserId = "test-user-1"
        };
        private readonly List<UserAccount> _accountList = new List<UserAccount>
        {
            new UserAccount { Id = "test-account-1", Name = "Test1", Server = "s1", UserId = "test-user-1"},
            _account,
            new UserAccount { Id = "test-account-3", Name = "Test3", Server = "s3", UserId = "test-user-1" }
        };

        [TestMethod]
        public void Render_Empty()
        {
            // arrange
            var ctx = new b.TestContext();

            // act
            var comp = ctx.RenderComponent<AccountPicker>();

            // assert
            var expectedHtml =
@"  <div>Switch Account:</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render()
        {
            // arrange
            var ctx = new b.TestContext();

            // act
            var comp = ctx.RenderComponent<AccountPicker>(parameters => parameters
                .Add(p => p.CurrentUserAccount, _account)
                .Add(p => p.UserAccounts, _accountList));

            // assert
            var expectedHtml =
@"
<div>Switch Account:</div>
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
        <button id=""switch-account-btn"" class=""btn btn-primary"" disabled=""""  >
          Switch Account
        </button>
      </td>
    </tr>
  </tfoot>
</table>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void SwitchAccount()
        {
            // arrange
            bool eventFired = false;
            var ctx = new b.TestContext();
            var comp = ctx.RenderComponent<AccountPicker>(parameters => parameters
                .Add(p => p.CurrentUserAccount, _account)
                .Add(p => p.UserAccounts, _accountList)
                .Add(p => p.SelectedAccountChanged, (acc) => eventFired = true));

            // act
            comp.Find("#test-account-3").Click();
            comp.Find("#switch-account-btn").Click();

            // assert
            var expectedHtml =
@"
<div>Switch Account:</div>
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
      <td >
        <span class=""oi oi-check glyph-mark"" ></span>
      </td>
      <td >Test2</td>
      <td class=""text-end"" >s1</td>
    </tr>
    <tr id=""test-account-3"" class=""selected"" >
      <td ></td>
      <td >Test3</td>
      <td class=""text-end"" >s3</td>
    </tr>
  </tbody>
  <tfoot >
    <tr >
      <td class=""py-2"" colspan=""3"" >
        <button id=""switch-account-btn"" class=""btn btn-primary"">
          Switch Account
        </button>
      </td>
    </tr>
  </tfoot>
</table>
";
            comp.MarkupMatches(expectedHtml);
            Assert.IsTrue(eventFired);
        }
    }
}
