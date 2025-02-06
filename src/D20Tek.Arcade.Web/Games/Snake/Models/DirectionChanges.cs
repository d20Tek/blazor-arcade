using D20Tek.Arcade.Web.Games.Snake.Model;

namespace D20Tek.Arcade.Web.Games.Snake.Models;

internal class DirectionChanges
{
    private readonly LinkedList<Direction> _dirChanges = new();

    public void ChangeDirection(Direction newDirection, Direction currentDirection)
    {
        if (CanChangeDirection(newDirection, currentDirection))
        {
            _dirChanges.AddLast(newDirection);
        }
    }

    private Direction GetLastDirection(Direction currentDirection) =>
        _dirChanges.Count == 0 ? currentDirection : _dirChanges.Last!.Value;

    private bool CanChangeDirection(Direction newDir, Direction currentDirection)
    {
        if (_dirChanges.Count >= 2) return false;

        var lastDir = GetLastDirection(currentDirection);
        return newDir != lastDir && newDir != lastDir.Opposite();
    }

    public Direction? GetNextDirection()
    {
        if (_dirChanges.Count > 0)
        {
            var direction = _dirChanges.First!.Value;
            _dirChanges.RemoveFirst();

            return direction;
        }

        return null;
    }

    public void Clear() => _dirChanges.Clear();
}
