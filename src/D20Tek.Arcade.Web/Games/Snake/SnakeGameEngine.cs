using D20Tek.Arcade.Web.Games.Snake.Model;

namespace D20Tek.Arcade.Web.Games.Snake;

internal class SnakeGameEngine
{
    private readonly Action _stateChanged;
    private readonly Func<int, Task> _levelChanged;
    private readonly GameState _gameState;
    private readonly string[,] _gridImages;

    public SnakeGameEngine(int rows, int columns, Action stateChangedAction, Func<int, Task> levelChanged)
    {
        _stateChanged = stateChangedAction;
        _levelChanged = levelChanged;

        _gridImages = new string[rows, columns];
        _gameState = new GameState(rows, columns);
    }

    public async Task RunGameAsync()
    {
        SnakeGridRenderer.Draw(_gameState, _gridImages, _stateChanged);

        await GameLoop();

        await SnakeGridRenderer.DrawDeadSnake(_gameState, _gridImages, _stateChanged);
    }

    public void ChangeDirection(Direction direction) => _gameState.ChangeDirection(direction);

    public int GetScore() => _gameState.Score;

    public int GetLevel() => _gameState.Level;

    public GridCellResponse GetGridCell(int row, int column) =>
        new(_gridImages[row, column], _gameState.Snake.GetHeadRotation(row, column));

    private async Task GameLoop()
    {
        while (!_gameState.GameOver)
        {
            await Task.Delay(_gameState.Speed);

            _gameState.Move();
            SnakeGridRenderer.Draw(_gameState, _gridImages, _stateChanged);

            await HandleNewLevel();
        }
    }

    private async Task HandleNewLevel()
    {
        if (_gameState.ChangeLevel())
        {
            await _levelChanged(_gameState.Level);
        }
    }
}
