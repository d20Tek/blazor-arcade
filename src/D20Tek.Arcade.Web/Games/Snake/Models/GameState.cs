using D20Tek.Arcade.Web.Games.Snake.Models;

namespace D20Tek.Arcade.Web.Games.Snake.Model;

internal class GameState
{
    private readonly DirectionChanges _dirChanges = new();
    private readonly LevelTracker _levelTracker = new();
    private Level _currentLevel;
    private int _consumedApples;

    public int Rows { get; }

    public int Cols { get; }

    public GridValue[,] Grid { get; private set; }

    public SnakeEntity Snake { get; private set; }

    public int Level => _currentLevel.Id;

    public int Speed => _currentLevel.Speed;

    public int Score { get; private set; }

    public bool GameOver { get; private set; }

    public GameState(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;

        _currentLevel = _levelTracker.GetNextLevel();
        InitializeForLevel();
    }

    public void ChangeDirection(Direction direction) => _dirChanges.ChangeDirection(direction, Snake.Direction);

    public bool OutsideGrid(Position pos) => pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;

    private GridValue WillHit(Position newHeadPos)
    {
        if (OutsideGrid(newHeadPos)) return GridValue.Outside;
        if (newHeadPos == Snake.TailPosition()) return GridValue.Empty;

        return Grid[newHeadPos.Row, newHeadPos.Col];
    }

    public void Move()
    {
        Snake.Direction = _dirChanges.GetNextDirection() ?? Snake.Direction;
        var newHeadPos = Snake.HeadPosition().Translate(Snake.Direction);
        PerformMove(newHeadPos, WillHit(newHeadPos));
    }

    private void PerformMove(Position newHeadPos, GridValue hit)
    {
        switch (hit)
        {
            case GridValue.Outside:
            case GridValue.Snake:
            case GridValue.Rock:
                GameOver = true;
                break;
            case GridValue.Empty:
                Snake.RemoveTail();
                Snake.AddHead(newHeadPos);
                break;
            case GridValue.Food:
                Snake.AddHead(newHeadPos);
                Score += _currentLevel.PointMultiplier;
                _consumedApples++;
                Grid.AddFood(Rows, Cols, 1);
                break;
        }
    }

    public bool ChangeLevel()
    {
        if (ShouldNotChangeLevel(_consumedApples)) return false;

        _currentLevel = _levelTracker.GetNextLevel();
        InitializeForLevel();
        return true;
    }

    private void InitializeForLevel()
    {
        Grid = new GridValue[Rows, Cols];
        _dirChanges.Clear();
        _consumedApples = 0;

        Snake = new(Grid);
        Snake.Add(Rows / 2, _currentLevel.StartingSnakeLength);

        Grid.AddFood(Rows, Cols, _currentLevel.Apples);
        Grid.AddRock(Rows, Cols, _currentLevel.Blocks);
    }

    private bool ShouldNotChangeLevel(int consumedApples) =>
        GameOver || (_currentLevel.ApplesToComplete < 0 || consumedApples < _currentLevel.ApplesToComplete);
}
