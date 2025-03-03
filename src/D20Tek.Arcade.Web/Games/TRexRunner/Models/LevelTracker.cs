namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class LevelTracker
{
    private static readonly Level[] _levels =
    [
        new(1, 300, false, 10, 10, 5),
        new(2, 300, false, 20, 10, 7),
        new(3, 300, false, 30, 10, 9),
        new(4, 300, false, 40, 10, 12),
        new(5, 300, true, 50, 10, 15),
        new(5, 300, true, 60, 20, 17),
        new(5, 300, true, 70, 20, 18),
        new(5, 300, true, 80, 20, 20),
        new(5, 300, true, 90, 20, 22),
        new(5, 300, true, 100, -1, 25),
    ];

    private int _currentLevel = 0;

    public Level GetNextLevel() => _levels[_currentLevel++];
}
