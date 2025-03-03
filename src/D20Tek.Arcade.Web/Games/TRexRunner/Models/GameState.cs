using D20Tek.Arcade.Web.Common;
using System.Diagnostics;

namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class GameState
{
    private static readonly int _numObstacleTypes = EnumExtensions.Count<Obstacle.Type>();
    private readonly LevelTracker _levelTracker = new();
    private Level _currentLevel;
    private int _clearedObstacles = 0;

    public LayoutData Layout { get; private set; }

    public IRandomRoller Rnd { get; }

    public int Score { get; private set; }

    public int Level => _currentLevel.Id;

    public int Speed => _currentLevel.ObstacleSpeed;

    public bool GameOver { get; private set; }

    private GameState(IRandomRoller rnd, LayoutSize layoutSize)
    {
        Layout = LayoutUpdated(layoutSize);
        Rnd = rnd;
        Score = 0;
        _currentLevel = _levelTracker.GetNextLevel();
    }

    public static GameState Create(IRandomRoller rnd, LayoutSize layoutSize = LayoutSize.Large) => new(rnd, layoutSize);

    public void IncrementScore(int amount)
    {
        Score += amount * _currentLevel.PointMultiplier;
        _clearedObstacles++;
    }

    public LayoutData LayoutUpdated(LayoutSize layoutSize) => Layout = LayoutConstants.GetLayout(layoutSize);

    public void SetGameOver(bool isGameOver) => GameOver = isGameOver;

    public int RollObstableSpawnRange() => Rnd.Next(300, _currentLevel.SpawnInterval);

    public Obstacle.Type RollObstacleType()
    {
        var maxTypes = _currentLevel.HasPterodactyl ? _numObstacleTypes : _numObstacleTypes - 1;
        var type = Rnd.Next(0, maxTypes);
        Debug.Assert(type >= 0 && type <= _numObstacleTypes - 1);
        return (Obstacle.Type)type;
    }

    public bool ChangeLevel()
    {
        if (ShouldNotChangeLevel(_clearedObstacles)) return false;

        _currentLevel = _levelTracker.GetNextLevel();
        _clearedObstacles = 0;
        return true;
    }

    private bool ShouldNotChangeLevel(int clearedObstacles) =>
        GameOver || (_currentLevel.ObstaclesToComplete < 0 || clearedObstacles < _currentLevel.ObstaclesToComplete);
}
