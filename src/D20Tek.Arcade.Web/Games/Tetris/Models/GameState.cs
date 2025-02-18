namespace D20Tek.Arcade.Web.Games.Tetris.Models;

internal class GameState
{
    public int Rows { get; }

    public int Columns { get; }

    public GameGrid GameGrid { get; }

    public BlockQueue BlockQueue { get; }

    public Block CurrentBlock { get; private set; }

    public bool GameOver { get; private set; }

    public int Score { get; private set; }

    public int Level { get; private set; } = 1;

    public GameState(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        GameGrid = new(rows, columns);
        BlockQueue = new();
        CurrentBlock = BlockQueue.GetAndUpdate();
    }

    private bool BlockFits() =>
        CurrentBlock.TilePositions().All(p => GameGrid.IsEmpty(p.Row, p.Column));

    public void RotateClockwise()
    {
        CurrentBlock.RotateClockwise();
        if (!BlockFits())
        {
            CurrentBlock.RotateCounterClockwise();
        }
    }

    public void RotateCounterClockwise()
    {
        CurrentBlock.RotateCounterClockwise();
        if (!BlockFits())
        {
            CurrentBlock.RotateClockwise();
        }
    }

    public void MoveBlockLeft()
    {
        CurrentBlock.Move(0, -1);
        if (!BlockFits())
        {
            CurrentBlock.Move(0, 1);
        }
    }

    public void MoveBlockRight()
    {
        CurrentBlock.Move(0, 1);
        if (!BlockFits())
        {
            CurrentBlock.Move(0, -1);
        }
    }

    public void MoveBlockDown()
    {
        CurrentBlock.Move(1, 0);

        if (!BlockFits())
        {
            CurrentBlock.Move(-1, 0);
            PlaceBlock();
        }
    }

    private bool IsGameOver() => !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));

    private void PlaceBlock()
    {
        CurrentBlock.TilePositions().ToList().ForEach(p => GameGrid[p.Row, p.Column] = CurrentBlock.Id);

        Score += ScoreTracker.Calculate(GameGrid.ClearFullRows(), Level);

        if (IsGameOver())
        {
            GameOver = true;
        }
        else
        {
            CurrentBlock = BlockQueue.GetAndUpdate();
        }
    }
}
