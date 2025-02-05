namespace D20Tek.Arcade.Web.Games.Snake.Model;

internal class GameState
{
    private readonly LinkedList<Direction> _dirChanges = new();
    private readonly LinkedList<Position> _snakePositions = new();
    private readonly Random _random = new();

    public int Rows { get; }

    public int Cols { get; }

    public GridValue[,] Grid { get; }

    public Direction Direction { get; private set; }

    public int Score { get; private set; }

    public bool GameOver { get; private set; }

    public GameState(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;
        Grid = new GridValue[Rows, Cols];
        Direction = Direction.Right;

        AddSnake();
        AddFood();
    }

    private void AddSnake()
    {
        int r = Rows / 2;

        for (int c = 1; c <= 3; c++)
        {
            Grid[r, c] = GridValue.Snake;
            _snakePositions.AddFirst(new Position(r, c));
        }
    }

    private IEnumerable<Position> EmptyPositions()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                if (Grid[r, c] == GridValue.Empty)
                {
                    yield return new Position(r, c);
                }
            }
        }
    }

    private void AddFood()
    {
        var empty = EmptyPositions().ToList();
        if (empty.Count == 0)
        {
            return;
        }

        var pos = empty[_random.Next(empty.Count)];
        Grid[pos.Row, pos.Col] = GridValue.Food;
    }

    public Position HeadPosition() => _snakePositions.First!.Value;

    public Position TailPosition() => _snakePositions.Last!.Value;

    public IEnumerable<Position> SnakePositions() => _snakePositions;

    private void AddHead(Position pos)
    {
        _snakePositions.AddFirst(pos);
        Grid[pos.Row, pos.Col] = GridValue.Snake;
    }

    private void RemoveTail()
    {
        var tail = _snakePositions.Last!.Value;
        Grid[tail.Row, tail.Col] = GridValue.Empty;
        _snakePositions.RemoveLast();
    }

    public void ChangeDirection(Direction direction)
    {
        if (CanChangeDirection(direction))
        {
            _dirChanges.AddLast(direction);
        }
    }

    private Direction GetLastDirection() => _dirChanges.Count == 0 ? Direction : _dirChanges.Last!.Value;

    private bool CanChangeDirection(Direction newDir)
    {
        if (_dirChanges.Count >= 2) return false;

        var lastDir = GetLastDirection();
        return newDir != lastDir && newDir != lastDir.Opposite();
    }

    public bool OutsideGrid(Position pos) => pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;

    private GridValue WillHit(Position newHeadPos)
    {
        if (OutsideGrid(newHeadPos)) return GridValue.Outside;
        if (newHeadPos == TailPosition()) return GridValue.Empty;

        return Grid[newHeadPos.Row, newHeadPos.Col];
    }

    public void Move()
    {
        if (_dirChanges.Count > 0)
        {
            Direction = _dirChanges.First!.Value;
            _dirChanges.RemoveFirst();
        }

        var newHeadPos = HeadPosition().Translate(Direction);
        var hit = WillHit(newHeadPos);

        if (hit == GridValue.Outside || hit == GridValue.Snake)
        {
            GameOver = true;
        }
        else if (hit == GridValue.Empty)
        {
            RemoveTail();
            AddHead(newHeadPos);
        }
        else if (hit == GridValue.Food)
        {
            AddHead(newHeadPos);
            Score++;
            AddFood();
        }
    }
}
