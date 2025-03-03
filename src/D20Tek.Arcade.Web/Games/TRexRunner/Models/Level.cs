namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal record Level(
    int Id,
    int SpawnInterval,
    bool HasPterodactyl,
    int PointMultiplier,
    int ObstaclesToComplete,
    int Speed);
