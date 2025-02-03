using Microsoft.AspNetCore.Components;

namespace D20Tek.Arcade.Web.Games.Snake;

public partial class GameGrid
{
    private readonly Dictionary<Direction, int> _dirToRotation = new()
    {
        { Direction.Up, 0 },
        { Direction.Right, 90 },
        { Direction.Down, 180 },
        { Direction.Left, 270 }
    };

    private GameState _gameState;
    private readonly string[,] _gridImages;

    public GameGrid()
    {
        _gridImages = new string[Rows, Columns];
        _gameState = new GameState(Rows, Columns);
    }

    [Parameter]
    public int Rows { get; set; }

    [Parameter]
    public int Columns { get; set; }

    private string? GetCellStyle(int row, int col)
    {
        var headPos = _gameState.HeadPosition();
        var rotation = (row == headPos.Row && col == headPos.Col) ? _dirToRotation[_gameState.Direction] : 0;

        return !string.IsNullOrEmpty(_gridImages[row, col])
            ? $"background-image: url('{_gridImages[row, col]}'); background-size: cover; transform: rotate({rotation}deg)"
            : null;
    }
}
