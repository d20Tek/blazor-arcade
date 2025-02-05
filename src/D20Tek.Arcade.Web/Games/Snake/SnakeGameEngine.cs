using D20Tek.Arcade.Web.Games.Snake.Model;

namespace D20Tek.Arcade.Web.Games.Snake;

internal class SnakeGameEngine
{
    private int _rows;
    private int _columns;
    private Action _stateChanged;
    private GameState _gameState = new(15, 15);
    private string[,] _gridImages = new string[15, 15];

    public SnakeGameEngine(int rows, int columns, Action stateChangedAction)
    {
        _rows = rows;
        _columns = columns;
        _stateChanged = stateChangedAction;
    }

    public async Task RunGameAsync()
    {
        InitializeLevel();

        SnakeGridRenderer.Draw(_gameState, _gridImages, _stateChanged);

        await GameLoop();

        await SnakeGridRenderer.DrawDeadSnake(_gameState, _gridImages, _stateChanged);
    }

    public void ChangeDirection(Direction direction) => _gameState.ChangeDirection(direction);

    public int GetScore() => _gameState.Score;

    public int GetHeadRotation(int row, int column)
    {
        var headPos = _gameState.HeadPosition();
        return (row == headPos.Row && column == headPos.Col)
                     ? HeadRotationMapper.GetRotation(_gameState.Direction)
                     : 0;
    }

    public string GetGridImage(int row, int column) => _gridImages[row, column];

    private void InitializeLevel()
    {
        _gridImages = new string[_rows, _columns];
        _gameState = new GameState(_rows, _columns);
    }

    private async Task GameLoop()
    {
        while (!_gameState.GameOver)
        {
            await Task.Delay(150);
            _gameState.Move();
            SnakeGridRenderer.Draw(_gameState, _gridImages, _stateChanged);
        }
    }
}
