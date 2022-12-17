//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Components;
using Blazor.Arcade.Common.Models;
using Bunit;

namespace Blazor.Arcade.Client.UnitTests.Components
{
    [TestClass]
    public class ProfilePickerTests
    {
        private static readonly UserProfile _account = new UserProfile
        {
            Id = "test-account-2",
            Name = "Test2",
            Server = "s1",
            UserId = "test-user-1"
        };
        private readonly List<UserProfile> _accountList = new List<UserProfile>
        {
            new UserProfile { Id = "test-account-1", Name = "Test1", Server = "s1", UserId = "test-user-1"},
            _account,
            new UserProfile { Id = "test-account-3", Name = "Test3", Server = "s3", UserId = "test-user-1" }
        };

        [TestMethod]
        public void Render_Empty()
        {
            // arrange
            var ctx = new b.TestContext();

            // act
            var comp = ctx.RenderComponent<ProfilePicker>();

            // assert
            var expectedHtml =
@"  <div>Switch Profile:</div>
    <div >You don't have any profiles set up yet. Create a player profile.</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render()
        {
            // arrange
            var ctx = new b.TestContext();

            // act
            var comp = ctx.RenderComponent<ProfilePicker>(parameters => parameters
                .Add(p => p.CurrentProfile, _account)
                .Add(p => p.UserProfiles, _accountList));

            // assert
            var expectedHtml =
@"
<div>Switch Profile:</div>
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
        <button id=""switch-profile-btn"" class=""btn btn-primary"" disabled=""""  >
          Switch Profile
        </button>
      </td>
    </tr>
  </tfoot>
</table>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void SwitchProfile()
        {
            // arrange
            bool eventFired = false;
            var ctx = new b.TestContext();
            var comp = ctx.RenderComponent<ProfilePicker>(parameters => parameters
                .Add(p => p.CurrentProfile, _account)
                .Add(p => p.UserProfiles, _accountList)
                .Add(p => p.SelectedProfileChanged, (acc) => eventFired = true));

            // act
            comp.Find("#test-account-3").Click();
            comp.Find("#switch-profile-btn").Click();

            // assert
            var expectedHtml =
@"
<div>Switch Profile:</div>
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
        <button id=""switch-profile-btn"" class=""btn btn-primary"">
          Switch Profile
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
