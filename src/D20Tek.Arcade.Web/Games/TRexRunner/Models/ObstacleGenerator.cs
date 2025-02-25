using D20Tek.Arcade.Web.Common;
using System.Diagnostics;

namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal static class ObstacleGenerator
{
    private const int _percentFlyingHigh = 60;
    private const int _flyingMultiplier = 3;
    private static readonly int _numObstacleTypes = EnumExtensions.Count<Obstacle.Type>();

    public static Obstacle Create(GameState state)
    {
        var type = GenerateRandomType(state);
        var typeSize = state.Layout.Obstacles[(int)type];
        var bounds = new Rectangle(
            state.Layout.Viewport.Width,
            CalculateStartingTop(state, type, typeSize),
            typeSize.Width,
            typeSize.Height);

        return Obstacle.Create(bounds, type);
    }

    private static Obstacle.Type GenerateRandomType(GameState state)
    {
        var type = state.Rnd.Next(0, _numObstacleTypes);
        Debug.Assert(type >= 0 && type <= _numObstacleTypes - 1);
        return (Obstacle.Type)type;
    }

    private static int CalculateStartingTop(GameState state, Obstacle.Type type, Size typeSize) =>
        type switch
        {
            Obstacle.Type.Pterodactyl when state.Rnd.Next(0, 100) > _percentFlyingHigh =>
                 state.Layout.Viewport.Height - LayoutConstants.BottomMargin - (_flyingMultiplier * typeSize.Height),

            _ => state.Layout.Viewport.Height - LayoutConstants.BottomMargin - typeSize.Height
        };
}
