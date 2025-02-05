namespace D20Tek.Arcade.Web.Games.Snake;

internal class SnakeGameEngine
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

    private int _rows;
    private int _columns;
    private Action _stateChanged;

    public GameState GameState { get; set; } = new(15, 15);

    public string[,] GridImages = new string[15, 15];

    private SnakeGameEngine(int rows, int columns, Action stateChangedAction)
    {
        _rows = rows;
        _columns = columns;
        _stateChanged = stateChangedAction;
    }

    public static SnakeGameEngine Create(int rows, int columns, Action stateChangedAction) =>
        new(rows, columns, stateChangedAction);

    public async Task RunGameAsync()
    {
        InitializeLevel();

        Draw();

        await GameLoop();

        await DrawDeadSnake();
    }

    public void ChangeDirection(Direction direction) => GameState.ChangeDirection(direction);

    public int GetHeadRotation(int row, int column)
    {
        var headPos = GameState.HeadPosition();
        var rotation = (row == headPos.Row && column == headPos.Col) ? _dirToRotation[GameState.Direction] : 0;

        return rotation;
    }

    private void InitializeLevel()
    {
        GridImages = new string[_rows, _columns];
        GameState = new GameState(_rows, _columns);
    }

    private async Task GameLoop()
    {
        while (!GameState.GameOver)
        {
            await Task.Delay(150);
            GameState.Move();
            Draw();
        }
    }

    private void Draw()
    {
        DrawGrid();

        var headPos = GameState.HeadPosition();
        GridImages[headPos.Row, headPos.Col] = Images.Head;

        _stateChanged();
    }

    private void DrawGrid()
    {
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                var gridVal = GameState.Grid[r, c];
                GridImages[r, c] = _gridValToImage[gridVal];
            }
        }
    }

    private async Task DrawDeadSnake()
    {
        var positions = GameState.SnakePositions().ToList();
        for (int i = 0; i < positions.Count; i++)
        {
            var pos = positions[i];
            var source = (i == 0) ? Images.DeadHead : Images.DeadBody;
            GridImages[pos.Row, pos.Col] = source;

            _stateChanged();
            await Task.Delay(50);
        }

        await Task.Delay(250);
    }
}
