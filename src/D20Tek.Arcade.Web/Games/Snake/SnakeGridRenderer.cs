using D20Tek.Arcade.Web.Games.Snake.Model;

namespace D20Tek.Arcade.Web.Games.Snake;

internal static class SnakeGridRenderer
{
    public static void Draw(GameState state, string[,] gridImages, Action stateChanged)
    {
        DrawGrid(state, gridImages);

        var headPos = state.HeadPosition();
        gridImages[headPos.Row, headPos.Col] = Images.Head;

        stateChanged();
    }

    private static void DrawGrid(GameState state, string[,] gridImages)
    {
        for (int r = 0; r < state.Rows; r++)
        {
            for (int c = 0; c < state.Cols; c++)
            {
                var gridVal = state.Grid[r, c];
                gridImages[r, c] = GridValueMapper.GetImage(gridVal);
            }
        }
    }

    public static async Task DrawDeadSnake(GameState state, string[,] gridImages, Action stateChanged)
    {
        var positions = state.SnakePositions().ToList();
        for (int i = 0; i < positions.Count; i++)
        {
            var pos = positions[i];
            var source = (i == 0) ? Images.DeadHead : Images.DeadBody;
            gridImages[pos.Row, pos.Col] = source;

            stateChanged();
            await Task.Delay(50);
        }

        await Task.Delay(250);
    }
}
