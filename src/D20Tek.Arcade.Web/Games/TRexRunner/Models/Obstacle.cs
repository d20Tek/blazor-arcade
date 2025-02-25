using System.Diagnostics;

namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class Obstacle
{
    internal enum Type
    {
        Cactus1,
        Cactus2,
        Cactus3,
        Pterodactyl
    }

    private int _speed = 5;
    private Type _type;

    public Rectangle Bounds { get; private set; }

    public Obstacle(GameState state, int type)
    {
        Debug.Assert(type >= 0 && type <= 3);
        _type = (Type)type;

        var typeSize = state.Layout.Obstacles[type];
        Bounds = new(
            state.Layout.Viewport.Width,
            state.Layout.Viewport.Height - LayoutConstants.BottomMargin - typeSize.Height,
            typeSize.Width,
            typeSize.Height);
    }

    public string GetImage() =>
        _type switch
        {
            Type.Cactus1 => "assets/trex/cactus1.png",
            Type.Cactus2 => "assets/trex/cactus2.png",
            Type.Cactus3 => "assets/trex/cactus3.png",
            Type.Pterodactyl => "assets/trex/pteryl-flying.gif",
            _ => throw new InvalidOperationException(),
        };

    public void Move()
    {
        Bounds.Translate(-_speed, 0);
    }
}
