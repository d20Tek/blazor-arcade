namespace D20Tek.Arcade.Web.Games.Tetris.Models;

internal class LevelTracker
{
    private static readonly Level[] _levels =
    [
        new(1, 1, 10, 300),
        new(2, 2, 10, 275),
        new(3, 3, 10, 250),
        new(4, 4, 10, 225),
        new(5, 5, 25, 200),
        new(6, 6, 25, 175),
        new(7, 7, 25, 150),
        new(8, 8, 25, 125),
        new(9, 9, 25, 100),
        new(10, 10, 25, 75),
        new(11, 11, 25, 60),
        new(12, 12, -1, 50),
    ];

    private int _currentLevel = 0;

    public Level GetNextLevel() => _levels[_currentLevel++];
}
