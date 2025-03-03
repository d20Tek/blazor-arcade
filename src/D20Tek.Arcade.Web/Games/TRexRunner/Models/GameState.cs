using D20Tek.Arcade.Web.Common;

namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class GameState
{
    private readonly LevelTracker _levelTracker = new();
    private Level _currentLevel;
    private int _clearedObstacles = 0;

    public LayoutData Layout { get; private set; }

    public IRandomRoller Rnd { get; }

    public int Score { get; private set; }

    public int Level => _currentLevel.Id;

    public int Speed => _currentLevel.Speed;

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
