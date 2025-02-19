using D20Tek.Arcade.Web.Games.Tetris.Models;

namespace D20Tek.Arcade.Web.Games.Tetris;

internal class TetrisGameEngine
{
    private readonly Action _stateChanged;
    private readonly Func<int, Task> _levelChanged;
    private readonly GameState _gameState;
    private readonly string[,] _gridImages;

    public TetrisGameEngine(int rows, int columns, Action stateChangedAction, Func<int, Task> levelChanged)
    {
        _stateChanged = stateChangedAction;
        _levelChanged = levelChanged;

        _gridImages = new string[rows, columns];
        _gameState = new GameState(rows, columns);
    }

    public bool GameOver => _gameState.GameOver;

    public async Task RunGameAsync()
    {
        TetrisGridRenderer.Draw(_gameState, _gridImages, _stateChanged);
        await GameLoop();
    }

    public int GetLevel() => _gameState.Level;

    public int GetScore() => _gameState.Score;

    public string GetTileImage(int row, int column) => _gridImages[row, column];

    public string GetNextBlockImage() => BlockFactory.GetBlockImage(_gameState.BlockQueue.NextBlock.Id);

    public void MoveLeft() => _gameState.MoveBlockLeft();

    public void MoveRight() => _gameState.MoveBlockRight();

    public void MoveDown() => _gameState.MoveBlockDown();

    public void MoveDrop() => _gameState.MoveBlockDrop();

    public void Rotate() => _gameState.RotateClockwise();

    public void RotateCounter() => _gameState.RotateCounterClockwise();

    public void Draw() => TetrisGridRenderer.Draw(_gameState, _gridImages, _stateChanged);

    private async Task GameLoop()
    {
        while (!_gameState.GameOver)
        {
            await Task.Delay(_gameState.Speed);

            _gameState.MoveBlockDown();
            Draw();

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
