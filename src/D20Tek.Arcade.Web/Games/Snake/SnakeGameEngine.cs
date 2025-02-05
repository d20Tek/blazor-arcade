using D20Tek.Arcade.Web.Games.Snake.Model;

namespace D20Tek.Arcade.Web.Games.Snake;

internal class SnakeGameEngine
{
    private int _rows;
    private int _columns;
    private Action _stateChanged;
    private Func<int, Task> _levelChanged;
    private GameState _gameState = new(15, 15);
    private string[,] _gridImages = new string[15, 15];

    public SnakeGameEngine(int rows, int columns, Action stateChangedAction, Func<int, Task> levelChanged)
    {
        _rows = rows;
        _columns = columns;
        _stateChanged = stateChangedAction;
        _levelChanged = levelChanged;
    }

    public async Task RunGameAsync()
    {
        Initialize();

        SnakeGridRenderer.Draw(_gameState, _gridImages, _stateChanged);

        await GameLoop();

        await SnakeGridRenderer.DrawDeadSnake(_gameState, _gridImages, _stateChanged);
    }

    public void ChangeDirection(Direction direction) => _gameState.ChangeDirection(direction);

    public int GetScore() => _gameState.Score;

    public int GetLevel() => _gameState.Level;

    public int GetHeadRotation(int row, int column)
    {
        var headPos = _gameState.HeadPosition();
        return (row == headPos.Row && column == headPos.Col)
                     ? HeadRotationMapper.GetRotation(_gameState.Direction)
                     : 0;
    }

    public string GetGridImage(int row, int column) => _gridImages[row, column];

    private void Initialize()
    {
        _gridImages = new string[_rows, _columns];
        _gameState = new GameState(_rows, _columns);
    }

    private async Task GameLoop()
    {
        while (!_gameState.GameOver)
        {
            await Task.Delay(_gameState.Speed);

            await HandleNewLevel();

            _gameState.Move();
            SnakeGridRenderer.Draw(_gameState, _gridImages, _stateChanged);
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
