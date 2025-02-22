namespace D20Tek.Arcade.Web.Games.TRexRunner;

internal class TrexGameEngine
{
    public int Level { get; private set; } = 1;

    public int Score { get; private set; } = 0;

    public bool GameOver { get; private set; } = false;
}
