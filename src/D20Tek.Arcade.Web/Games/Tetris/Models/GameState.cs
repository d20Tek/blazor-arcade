namespace D20Tek.Arcade.Web.Games.Tetris.Models;

internal class GameState
{
    private readonly LevelTracker _levelTracker = new();
    private Level _currentLevel;
    private int _clearedRows = 0;

    public int Rows { get; }

    public int Columns { get; }

    public GameGrid GameGrid { get; }

    public BlockQueue BlockQueue { get; }

    public Block CurrentBlock { get; private set; }

    public bool GameOver { get; private set; }

    public int Score { get; private set; }

    public int Level => _currentLevel.Id;

    public int Speed => _currentLevel.Speed;

    public GameState(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        GameGrid = new(rows, columns);
        BlockQueue = new();
        CurrentBlock = BlockQueue.GetAndUpdate();
        _currentLevel = _levelTracker.GetNextLevel();
    }

    internal void PlaceBlock()
    {
        CurrentBlock.TilePositions().ToList().ForEach(p => GameGrid[p.Row, p.Column] = CurrentBlock.Id);

        var clearedRows = GameGrid.ClearFullRows();
        Score += ScoreTracker.Calculate(clearedRows, Level);
        _clearedRows += clearedRows;

        if (IsGameOver())
        {
            GameOver = true;
        }
        else
        {
            CurrentBlock = BlockQueue.GetAndUpdate();
        }
    }

    private bool IsGameOver() => !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));

    public bool ChangeLevel()
    {
        if (ShouldNotChangeLevel(_clearedRows)) return false;

        _currentLevel = _levelTracker.GetNextLevel();
        _clearedRows = 0;
        return true;
    }

    private bool ShouldNotChangeLevel(int consumedApples) =>
        GameOver || (_currentLevel.RowsToComplete < 0 || consumedApples < _currentLevel.RowsToComplete);
}
