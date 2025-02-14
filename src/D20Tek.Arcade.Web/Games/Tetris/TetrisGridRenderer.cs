using D20Tek.Arcade.Web.Games.Tetris.Models;

namespace D20Tek.Arcade.Web.Games.Tetris;

internal static class TetrisGridRenderer
{
    public static void Draw(GameState state, string[,] gridImages, Action stateChanged)
    {
        DrawGrid(state, gridImages);
        DrawBlock(state.CurrentBlock, gridImages);
        stateChanged();
    }

    private static void DrawGrid(GameState state, string[,] gridImages)
    {
        for (int r = 0; r < state.Rows; r++)
        {
            for (int c = 0; c < state.Columns; c++)
            {
                var gridVal = state.GameGrid[r, c];
                gridImages[r, c] = BlockFactory.GetTileImage(gridVal);
            }
        }
    }

    private static void DrawBlock(Block block, string[,] gridImages)
    {
        foreach (var p in block.TilePositions())
        {
            gridImages[p.Row, p.Column] = BlockFactory.GetTileImage(block.Id);
        }
    }
}
