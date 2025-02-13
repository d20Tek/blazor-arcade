namespace D20Tek.Arcade.Web.Games.Tetris.Models;

internal class GameState
{
    public GameGrid GameGrid { get; }

    public BlockQueue BlockQueue { get; }

    public Block CurrentBlock { get; private set; }

    public bool GameOver { get; private set; }

    public GameState(int rows, int columns)
    {
        GameGrid = new(rows, columns);
        BlockQueue = new();
        CurrentBlock = BlockQueue.GetAndUpdate();
    }

    private bool BlockFits()
    {
        foreach (var p in CurrentBlock.TilePositions())
        {
            if (!GameGrid.IsEmpty(p.Row, p.Column))
            {
                return false;
            }
        }

        return true;
    }

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
        foreach (var p in CurrentBlock.TilePositions())
        {
            GameGrid[p.Row, p.Column] = CurrentBlock.Id;
        }

        GameGrid.ClearFullRows();

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
