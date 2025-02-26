using D20Tek.Arcade.Web.Common;

namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class GameState
{
    public LayoutData Layout { get; private set; }

    public IRandomRoller Rnd { get; }

    public int Score { get; private set; }

    private GameState(IRandomRoller rnd, LayoutSize layoutSize)
    {
        Layout = LayoutUpdated(layoutSize);
        Rnd = rnd;
        Score = 0;
    }

    public void IncrementScore(int amount) => Score += amount;

    public LayoutData LayoutUpdated(LayoutSize layoutSize) => Layout = LayoutConstants.GetLayout(layoutSize);

    public static GameState Create(IRandomRoller rnd, LayoutSize layoutSize = LayoutSize.Large) => new(rnd, layoutSize);
}
