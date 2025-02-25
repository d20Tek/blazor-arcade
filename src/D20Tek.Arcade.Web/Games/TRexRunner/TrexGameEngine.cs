using D20Tek.Arcade.Web.Games.TRexRunner.Models;

namespace D20Tek.Arcade.Web.Games.TRexRunner;

internal class TrexGameEngine
{
    private readonly GameState _state;

    public int Level { get; private set; } = 1;

    public int Score => _state.Score;

    public bool GameOver { get; private set; } = false;

    public DinoPlayer Dino { get; }

    public Obstacles Obstacles { get; } = new();

    public TrexGameEngine(GameState state)
    {
        _state = state;
        Dino = new(_state);
    }

    public async Task GameLoop(Action stateChangedAction)
    {
        while (!GameOver)
        {
            Dino.Move();
            Obstacles.GenerateObstacles(_state);
            Obstacles.Move(_state);

            GameOver = Dino.DetectCollision(Obstacles);

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
