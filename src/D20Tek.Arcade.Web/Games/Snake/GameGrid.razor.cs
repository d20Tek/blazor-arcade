using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace D20Tek.Arcade.Web.Games.Snake;

public partial class GameGrid
{
    private readonly Dictionary<GridValue, string> _gridValToImage = new()
    {
        { GridValue.Empty, Images.Empty },
        { GridValue.Snake, Images.Body },
        { GridValue.Food, Images.Food },
    };

    private readonly Dictionary<Direction, int> _dirToRotation = new()
    {
        { Direction.Up, 0 },
        { Direction.Right, 90 },
        { Direction.Down, 180 },
        { Direction.Left, 270 }
    };

    private GameState _gameState = new GameState(15, 15);
    private string[,] _gridImages = new string[15, 15] ;

    [Parameter]
    public int Rows { get; set; }

    [Parameter]
    public int Columns { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var dotNetRef = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("addKeyListener", dotNetRef);

            await RunGame();
        }
    }

    [JSInvokable]
    public void ChangeDirection(string key)
    {
        if (key == "ArrowUp") _gameState.ChangeDirection(Direction.Up);
        if (key == "ArrowDown") _gameState.ChangeDirection(Direction.Down);
        if (key == "ArrowLeft") _gameState.ChangeDirection(Direction.Left);
        if (key == "ArrowRight") _gameState.ChangeDirection(Direction.Right);
    }

    private async Task RunGame()
    {
        InitializeLevel();

        Draw();

        await GameLoop();

        await DrawDeadSnake();
        _gameState = new GameState(Rows, Columns);
    }

    private void InitializeLevel()
    {
        _gridImages = new string[Rows, Columns];
        _gameState = new GameState(Rows, Columns);
    }

    private async Task GameLoop()
    {
        while (!_gameState.GameOver)
        {
            await Task.Delay(150);
            _gameState.Move();
            Draw();
        }
    }

    private void Draw()
    {
        DrawGrid();

        var headPos = _gameState.HeadPosition();
        _gridImages[headPos.Row, headPos.Col] = Images.Head;

        StateHasChanged();
    }

    private void DrawGrid()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                var gridVal = _gameState.Grid[r, c];
                _gridImages[r, c] = _gridValToImage[gridVal];
            }
        }
    }

    private async Task DrawDeadSnake()
    {
        var positions = _gameState.SnakePositions().ToList();
        for (int i = 0; i < positions.Count; i++)
        {
            var pos = positions[i];
            var source = (i == 0) ? Images.DeadHead : Images.DeadBody;
            _gridImages[pos.Row, pos.Col] = source;

            StateHasChanged();
            await Task.Delay(50);
        }
    }

    private string? GetCellStyle(int row, int col)
    {
        var headPos = _gameState.HeadPosition();
        var rotation = (row == headPos.Row && col == headPos.Col) ? _dirToRotation[_gameState.Direction] : 0;

        return !string.IsNullOrEmpty(_gridImages[row, col])
            ? $"background-image: url('{_gridImages[row, col]}'); background-size: cover; transform: rotate({rotation}deg)"
            : null;
    }
}
