namespace D20Tek.Arcade.Web.Games.Snake.Models;

internal record Level(
    int Id,
    int Apples,
    int Blocks,
    int PointMultiplier,
    int ApplesToComplete,
    int StartingSnakeLength,
    int Speed);
