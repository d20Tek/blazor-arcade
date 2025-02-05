namespace D20Tek.Arcade.Web.Games.Snake.Models;

internal class LevelTracker
{
    private static readonly Level[] _levels =
    [
        new(1, 1, 0, 1, 10, 3, 150),
        new(2, 1, 0, 2, 10, 3, 140),
        new(3, 2, 0, 3, 10, 4, 130),
        new(4, 2, 0, 4, 10, 4, 120),
        new(5, 2, 1, 5, 15, 4, 110),
        new(6, 2, 2, 6, 15, 4, 100),
        new(7, 3, 3, 7, 15, 5, 90),
        new(8, 3, 4, 8, 20, 5, 80),
        new(9, 3, 4, 9, 20, 5, 70),
        new(10, 3, 5, 10, -1, 5, 60),
    ];

    private int _currentLevel = 0;

    public Level GetNextLevel() => _levels[_currentLevel++];
}
