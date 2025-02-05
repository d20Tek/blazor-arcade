namespace D20Tek.Arcade.Web.Games.Snake.Model;

internal static class HeadRotationMapper
{
    private static readonly Dictionary<Direction, int> _dirToRotation = new()
    {
        { Direction.Up, 0 },
        { Direction.Right, 90 },
        { Direction.Down, 180 },
        { Direction.Left, 270 }
    };

    public static int GetRotation(Direction direction) => _dirToRotation[direction];
}
