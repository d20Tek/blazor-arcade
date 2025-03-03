namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class LevelTracker
{
    private static readonly Level[] _levels =
    [
        new(1, 550, false, 10, 10, 5),
        new(2, 550, false, 20, 10, 6),
        new(3, 550, false, 30, 10, 7),
        new(4, 700, false, 40, 10, 8),
        new(5, 700, true, 50, 10, 9),
        new(6, 700, true, 60, 20, 10),
        new(7, 800, true, 70, 20, 12),
        new(8, 800, true, 80, 20, 14),
        new(9, 800, true, 90, 20, 16),
        new(10, 900, true, 100, -1, 18),
    ];

    private int _currentLevel = 0;

    public Level GetNextLevel() => _levels[_currentLevel++];
}
