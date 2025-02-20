namespace D20Tek.Arcade.Web.Games.Tetris.Models;

internal record Level(
    int Id,
    int PointMultiplier,
    int RowsToComplete,
    int Speed);
