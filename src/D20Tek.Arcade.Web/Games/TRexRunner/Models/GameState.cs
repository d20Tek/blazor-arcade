using System.Drawing;

namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class GameState
{
    public LayoutData Layout { get; }

    public GameState()
    {
        Layout = LayoutConstants.GetLayout(LayoutSize.Large);
    }
}
