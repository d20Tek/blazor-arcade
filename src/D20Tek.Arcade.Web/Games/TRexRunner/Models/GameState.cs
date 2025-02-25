namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class GameState
{
    public LayoutData Layout { get; }

    public Random Rnd { get; }

    public int Score { get; private set; }

    public GameState()
    {
        Layout = LayoutConstants.GetLayout(LayoutSize.Large);
        Rnd = new();
        Score = 0;
    }

    public void IncrementScore(int amount) => Score += amount;
}
