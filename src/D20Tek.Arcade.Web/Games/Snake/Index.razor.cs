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

    public Index()
    {
        _gridImages = new string[_rows, _cols];
        _gameState = new GameState(_rows, _cols);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await RunGame();
        }
    }

    private Task RunGame()
    {
        Draw();
        return Task.CompletedTask;

        //await ShowCountdown();
        //Overlay.Visibility = Visibility.Hidden;
        //await GameLoop();
        //await ShowGameOver();
        //_gameState = new GameState(_rows, _cols);
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

    private string? GetCellStyle(int row, int col)
    {
        var headPos = _gameState.HeadPosition();
        var rotation = (row == headPos.Row && col == headPos.Col) ? _dirToRotation[_gameState.Direction] : 0;

        return !string.IsNullOrEmpty(_gridImages[row, col])
            ? $"background-image: url('{_gridImages[row, col]}'); background-size: cover; transform: rotate({rotation}deg)"
            : null;
    }
}
