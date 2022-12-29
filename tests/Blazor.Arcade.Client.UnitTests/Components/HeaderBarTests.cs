//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Components;
using Blazor.Arcade.Common.Models;
using Bunit;

namespace Blazor.Arcade.Client.UnitTests.Components
{
    [TestClass]
    public class HeaderBarTests
    {
        [TestMethod]
        public void Render_NoTitle()
        {
            // arrange
            var ctx = new b.TestContext();

            // act
            var comp = ctx.RenderComponent<HeaderBar>();

            // assert
            var expectedHtml =
@"  
<div class=""header-top-bar"" >
  <h4 class=""px-3 header-brand"" >Header</h4>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_Title()
        {
            // arrange
            var ctx = new b.TestContext();

            // act
            var comp = ctx.RenderComponent<HeaderBar>(parameters => parameters
                .Add(p => p.Title, "Test Title"));

            // assert
            var expectedHtml =
@"  
<div class=""header-top-bar"" >
  <h4 class=""px-3 header-brand"" >Test Title</h4>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_ChildContent()
        {
            // arrange
            var ctx = new b.TestContext();

            // act
            var comp = ctx.RenderComponent<HeaderBar>(parameters => parameters
                .Add(p => p.Title, "Test Title")
                .AddChildContent("<button>Test</button>"));

            // assert
            var expectedHtml =
@"  
<div class=""header-top-bar"" >
  <div class=""px-3 float-end""><button>Test</button></div>
  <h4 class=""px-3 header-brand"" >Test Title</h4>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }
    }
}
