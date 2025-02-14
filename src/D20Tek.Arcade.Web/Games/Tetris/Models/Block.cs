namespace D20Tek.Arcade.Web.Games.Tetris.Models;

internal class Block
{
    private int _rotationState;
    private Position _offset;

    protected Position[][] Tiles { get; }

    protected Position StartOffset { get; }

    public int Id { get; }

    public Block(int id, Position[][] tiles, Position startOffset)
    {
        Id = id;
        Tiles = tiles;
        StartOffset = startOffset;
        _offset = new(StartOffset.Row, StartOffset.Column);
    }

    public IEnumerable<Position> TilePositions()
    {
        if (_rotationState < 0 || _rotationState >= Tiles.Length)
        {
            Console.WriteLine($"Invalid rotation state: {_rotationState}");
            yield break;
        }

        foreach (var p in Tiles[_rotationState])
        {
            yield return new(p.Row + _offset.Row, p.Column + _offset.Column);
        }
    }

    public void RotateClockwise() => _rotationState = (_rotationState + 1) % Tiles.Length;

    public void RotateCounterClockwise()
    {
        if (_rotationState == 0)
            _rotationState = Tiles.Length - 1;
        else
            _rotationState--;
    }

    public void Move(int rows, int columns) => _offset.Move(rows, columns);

    public void Reset()
    {
        _rotationState = 0;
        _offset = new(StartOffset.Row, StartOffset.Column);
    }
}
