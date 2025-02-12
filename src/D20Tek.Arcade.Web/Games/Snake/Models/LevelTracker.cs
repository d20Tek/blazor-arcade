namespace D20Tek.Arcade.Web.Games.Snake.Models;

internal class LevelTracker
{
    private static readonly Level[] _levels =
    [
        new(1, 1, 0, 1, 10, 3, 180),
        new(2, 1, 0, 2, 10, 3, 170),
        new(3, 2, 1, 3, 10, 4, 160),
        new(4, 2, 2, 4, 10, 4, 170),
        new(5, 2, 3, 5, 15, 4, 140),
        new(6, 2, 4, 6, 15, 4, 130),
        new(7, 3, 5, 7, 15, 5, 120),
        new(8, 3, 6, 8, 20, 5, 110),
        new(9, 3, 7, 9, 20, 5, 100),
        new(10, 3, 8, 10, -1, 5, 80),
    ];

    private int _currentLevel = 0;

    public Level GetNextLevel() => _levels[_currentLevel++];
}
