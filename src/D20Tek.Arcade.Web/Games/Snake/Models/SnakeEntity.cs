using D20Tek.Arcade.Web.Games.Snake.Model;

namespace D20Tek.Arcade.Web.Games.Snake.Models;

internal class SnakeEntity
{
    private readonly LinkedList<Position> _snakePositions = new();
    private readonly GridValue[,] _grid;

    public SnakeEntity(GridValue[,] grid)
    {
        _grid = grid;
    }

    public Direction Direction { get; set; } = Direction.Right;

    public Position HeadPosition() => _snakePositions.First!.Value;

    public Position TailPosition() => _snakePositions.Last!.Value;

    public IList<Position> GetPositions() => _snakePositions.ToList();

    public void Add(int startingRow, int length)
    {
        _snakePositions.Clear();
        int r = startingRow;

        for (int c = 1; c <= length; c++)
        {
            _grid[r, c] = GridValue.Snake;
            _snakePositions.AddFirst(new Position(r, c));
        }
    }

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

    public void AddHead(Position pos)
    {
        _snakePositions.AddFirst(pos);
        _grid[pos.Row, pos.Col] = GridValue.Snake;
    }

    public void RemoveTail()
    {
        var tail = _snakePositions.Last!.Value;
        _grid[tail.Row, tail.Col] = GridValue.Empty;
        _snakePositions.RemoveLast();
    }
}
