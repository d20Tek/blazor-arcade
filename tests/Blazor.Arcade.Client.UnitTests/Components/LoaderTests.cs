//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Components;
using Bunit;

namespace Blazor.Arcade.Client.UnitTests.Components
{
    [TestClass]
    public class LoaderTests
    {
        [TestMethod]
        public void Render_IsVisibleTrue()
        {
            // arrange
            var ctx = new b.TestContext();

            // act
            var comp = ctx.RenderComponent<Loader>(parameters => parameters
                .Add(p => p.IsVisible, true));

            // assert
            var expectedHtml =
@"
<div class=""text-center mt-3"">
  <span class=""spinner-border spinner-border-sm app-themed-spinner""
        role=""status"" aria-hidden=""true"" />
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_IsVisibleFalse()
        {
            // arrange
            var ctx = new b.TestContext();

            // act
            var comp = ctx.RenderComponent<Loader>(parameters => parameters
                .Add(p => p.IsVisible, false));

            // assert
            comp.MarkupMatches(string.Empty);
        }
    }
}
