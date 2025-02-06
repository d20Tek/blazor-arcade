using D20Tek.Arcade.Web.Games.Snake.Models;

namespace D20Tek.Arcade.Web.Games.Snake.Model;

internal class GameState
{
    private readonly DirectionChanges _dirChanges = new();
    private readonly LinkedList<Position> _snakePositions = new();
    private readonly Random _random = new();
    private readonly LevelTracker _levelTracker = new();
    private Level _currentLevel;
    private int _consumedApples;

    public int Rows { get; }

    public int Cols { get; }

    public GridValue[,] Grid { get; private set; }

    public Direction Direction { get; private set; }

    public int Level => _currentLevel.Id;

    public int Speed => _currentLevel.Speed;

    public int Score { get; private set; }

    public bool GameOver { get; private set; }

    public GameState(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;
        Grid = new GridValue[Rows, Cols];
        Direction = Direction.Right;
        _currentLevel = _levelTracker.GetNextLevel();

        AddSnake();
        AddFood(_currentLevel.Apples);
    }

    private void AddSnake()
    {
        _snakePositions.Clear();
        int r = Rows / 2;

        for (int c = 1; c <= _currentLevel.StartingSnakeLength; c++)
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

    private void AddFood(int amount)
    {
        var empty = EmptyPositions().ToList();
        if (empty.Count == 0)
        {
            return;
        }

        for (int a = 1; a <= amount; a++)
        {
            var pos = empty[_random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Food;

            empty.Remove(pos);
        }
    }

    private Position HeadPosition() => _snakePositions.First!.Value;

    private Position TailPosition() => _snakePositions.Last!.Value;

    public IEnumerable<Position> SnakePositions() => _snakePositions;

    public int GetHeadRotation(int row, int column)
    {
        var headPos = HeadPosition();
        return (row == headPos.Row && column == headPos.Col)
                    ? HeadRotationMapper.GetRotation(Direction)
                    : 0;
    }

    public void PlaceHead(string[,] gridImages)
    {
        var headPos = HeadPosition();
        gridImages[headPos.Row, headPos.Col] = Images.Head;
    }

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

    public void ChangeDirection(Direction direction) => _dirChanges.ChangeDirection(direction, Direction);

    public bool OutsideGrid(Position pos) => pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;

    private GridValue WillHit(Position newHeadPos)
    {
        if (OutsideGrid(newHeadPos)) return GridValue.Outside;
        if (newHeadPos == TailPosition()) return GridValue.Empty;

        return Grid[newHeadPos.Row, newHeadPos.Col];
    }

    public void Move()
    {
        Direction = _dirChanges.GetNextDirection() ?? Direction;
        var newHeadPos = HeadPosition().Translate(Direction);
        PerformMove(newHeadPos, WillHit(newHeadPos));
    }

    private void PerformMove(Position newHeadPos, GridValue hit)
    {
        switch (hit)
        {
            case GridValue.Outside:
            case GridValue.Snake:
                GameOver = true;
                break;
            case GridValue.Empty:
                RemoveTail();
                AddHead(newHeadPos);
                break;
            case GridValue.Food:
                AddHead(newHeadPos);
                Score += _currentLevel.PointMultiplier;
                _consumedApples++;
                AddFood(1);
                break;
        }
    }

    public bool ChangeLevel()
    {
        if (GameOver || _levelTracker.ShouldChangeLevel(_consumedApples) is false) return false;

        Grid = new GridValue[Rows, Cols];
        Direction = Direction.Right;
        _dirChanges.Clear();
        _consumedApples = 0;
        _currentLevel = _levelTracker.GetNextLevel();

        AddSnake();
        AddFood(_currentLevel.Apples);

        return true;
    }
}
