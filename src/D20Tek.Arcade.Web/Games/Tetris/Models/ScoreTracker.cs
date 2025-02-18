namespace D20Tek.Arcade.Web.Games.Tetris.Models;

internal static class ScoreTracker
{
    private static int[] _scoresPerRow = [40, 100, 300, 1200];

    public static int Calculate(int rowsCleared, int level) =>
        (rowsCleared == 0) ? 0 : _scoresPerRow[rowsCleared - 1] * level;
}
