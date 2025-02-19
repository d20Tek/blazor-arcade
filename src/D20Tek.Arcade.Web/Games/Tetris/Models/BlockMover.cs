namespace D20Tek.Arcade.Web.Games.Tetris.Models;

internal static class BlockMover
{
    private static bool BlockFits(this GameState state) =>
        state.CurrentBlock.TilePositions().All(p => state.GameGrid.IsEmpty(p.Row, p.Column));

    public static void RotateClockwise(this GameState state)
    {
        state.CurrentBlock.RotateClockwise();
        if (!state.BlockFits())
        {
            state.CurrentBlock.RotateCounterClockwise();
        }
    }

    public static void RotateCounterClockwise(this GameState state)
    {
        state.CurrentBlock.RotateCounterClockwise();
        if (!state.BlockFits())
        {
            state.CurrentBlock.RotateClockwise();
        }
    }

    public static void MoveBlockLeft(this GameState state)
    {
        state.CurrentBlock.Move(0, -1);
        if (!state.BlockFits())
        {
            state.CurrentBlock.Move(0, 1);
        }
    }

    public static void MoveBlockRight(this GameState state)
    {
        state.CurrentBlock.Move(0, 1);
        if (!state.BlockFits())
        {
            state.CurrentBlock.Move(0, -1);
        }
    }

    public static void MoveBlockDown(this GameState state)
    {
        state.CurrentBlock.Move(1, 0);

        if (!state.BlockFits())
        {
            state.CurrentBlock.Move(-1, 0);
            state.PlaceBlock();
        }
    }
}
