using D20Tek.Arcade.Web.Games.TRexRunner.Models;

namespace D20Tek.Arcade.Web.Games.TRexRunner;

internal class TrexGameEngine
{
    private const int _defaultRefreshRate = 30;
    private readonly GameState _state;

    public int Level => _state.Level;

    public int Score => _state.Score;

    public bool GameOver { get; private set; } = false;

    public InputController Input { get; }

    public DinoPlayer Dino { get; }

    public Obstacles Obstacles { get; }

    private TrexGameEngine(GameState state)
    {
        _state = state;
        Input = InputController.Create(this);
        Dino = DinoPlayer.Create(_state);
        Obstacles = Obstacles.Create();
    }

    public static TrexGameEngine Create(GameState state) => new(state);

    public async Task GameLoop(Action stateChangedAction)
    {
        while (!_state.GameOver)
        {
            Dino.Move();
            Obstacles.GenerateObstacles(_state);
            Obstacles.Move(_state);

            _state.SetGameOver(Dino.DetectCollision(Obstacles.ToList()));
            _state.ChangeLevel();

            stateChangedAction();

            await Task.Delay(_defaultRefreshRate);
        }
    }

    public void EndGame() => _state.SetGameOver(true);

    public void UpdateLayout(int width) =>
        UpdateEntitiesLayout(_state.Layout.Viewport.Width, _state.LayoutUpdated(LayoutConstants.LayoutSizeFromWidth(width)));

    private void UpdateEntitiesLayout(int oldWidth, LayoutData newLayout)
    {
        if (oldWidth != newLayout.Viewport.Width)
        {
            Dino.LayoutUpdated(newLayout);
            Obstacles.LayoutUpdated(newLayout);
        }
    }
}
