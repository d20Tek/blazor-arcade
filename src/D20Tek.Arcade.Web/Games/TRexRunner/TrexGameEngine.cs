using D20Tek.Arcade.Web.Games.TRexRunner.Models;

namespace D20Tek.Arcade.Web.Games.TRexRunner;

internal class TrexGameEngine
{
    public int Level { get; private set; } = 1;

    public int Score { get; private set; } = 0;

    public bool GameOver { get; private set; } = false;

    public DinoPlayer Dino { get; } = new();

    public async Task GameLoop(Action stateChangedAction)
    {
        while (!GameOver)
        {
            Dino.Move();
            // todo: spawn new obstacles.
            // todo: move obstacles.
            // todo: hit detection.

            stateChangedAction();
            await Task.Delay(30);
        }
    }

    public Task EndGame()
    {
        GameOver = true;
        return Task.CompletedTask;
    }
}
