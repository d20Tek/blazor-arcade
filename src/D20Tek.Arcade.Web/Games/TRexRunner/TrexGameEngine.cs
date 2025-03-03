using D20Tek.Arcade.Web.Games.TRexRunner.Models;

namespace D20Tek.Arcade.Web.Games.TRexRunner;

internal class TrexGameEngine
{
    private const int _defaultRefreshRate = 30;
    private readonly GameState _state;
    private readonly Func<int, Task> _levelChanged;

    public int Level => _state.Level;

    public int Score => _state.Score;

    public bool GameOver { get; private set; } = false;

    public InputController Input { get; }

    public DinoPlayer Dino { get; }

    public Obstacles Obstacles { get; }

    private TrexGameEngine(GameState state, Func<int, Task> levelChanged)
    {
        _state = state;
        Input = InputController.Create(this);
        Dino = DinoPlayer.Create(_state);
        Obstacles = Obstacles.Create();
        _levelChanged = levelChanged;
    }

    public static TrexGameEngine Create(GameState state, Func<int, Task> levelChanged) => new(state, levelChanged);

    public async Task GameLoop(Action stateChangedAction)
    {
        while (!_state.GameOver)
        {
            Dino.Move();
            Obstacles.GenerateObstacles(_state);
            Obstacles.Move(_state);

            _state.SetGameOver(Dino.DetectCollision(Obstacles.ToList()));

            stateChangedAction();

            await HandleNewLevel();
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

    private async Task HandleNewLevel()
    {
        if (_state.ChangeLevel())
        {
            await _levelChanged(_state.Level);
        }
    }
}
