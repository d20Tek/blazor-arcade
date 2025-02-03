using Microsoft.JSInterop;

namespace D20Tek.Arcade.Web.Games.Snake;

public partial class Index
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

    private readonly int _rows = 15, _cols = 15;
    private readonly string[,] _gridImages;
    private GameState _gameState;
    private bool _gameRunning;
    private string? _countDownText = null;
    private string? _gameOverText = null;

    private bool ShowCountDownText => !string.IsNullOrEmpty(_countDownText);
    private bool ShowGameOverText => !string.IsNullOrEmpty(_gameOverText);

    public Index()
    {
        _gridImages = new string[_rows, _cols];
        _gameState = new GameState(_rows, _cols);
    }

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
        Draw();

        await ShowCountdown();

        await GameLoop();
        await ShowGameOver();
        _gameState = new GameState(_rows, _cols);
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
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
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

    private async Task ShowCountdown()
    {
        for (int i = 3; i >= 1; i--)
        {
            _countDownText = $"Game starts in ... {i}";
            StateHasChanged() ;

            await Task.Delay(500);
        }

        _countDownText = null;
    }

    private async Task ShowGameOver()
    {
        await DrawDeadSnake();
        _gameOverText = "GAME OVER... WOULD YOU LIKE TO PLAY AGAIN?";
        StateHasChanged();
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
