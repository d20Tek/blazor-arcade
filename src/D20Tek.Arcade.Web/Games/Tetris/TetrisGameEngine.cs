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

    public async Task RunGameAsync()
    {
        TetrisGridRenderer.Draw(_gameState, _gridImages, _stateChanged);
        await GameLoop();
    }

    public int GetScore() => 0;

    public string GetTileImage(int row, int column) => _gridImages[row, column];

    private async Task GameLoop()
    {
        while (!_gameState.GameOver)
        {
            await Task.Delay(150);

            //_gameState.Move();
            TetrisGridRenderer.Draw(_gameState, _gridImages, _stateChanged);

            //await HandleNewLevel();
        }
    }
}
