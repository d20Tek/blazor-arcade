using System.Diagnostics;

namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal static class ObstacleGenerator
{
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
        var type = state.Rnd.Next(0, 4);
        Debug.Assert(type >= 0 && type <= 3);
        return (Obstacle.Type)type;
    }

    private static int CalculateStartingTop(GameState state, Obstacle.Type type, Size typeSize) =>
        type switch
        {
            Obstacle.Type.Pterodactyl when state.Rnd.Next(0, 100) > 60 =>
                state.Layout.Viewport.Height - LayoutConstants.BottomMargin - (3 * typeSize.Height),

            _ => state.Layout.Viewport.Height - LayoutConstants.BottomMargin - typeSize.Height
        };
}
