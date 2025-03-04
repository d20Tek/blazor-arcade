namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class Obstacle : IGameEntity
{
    internal enum Type
    {
        Cactus1,
        Cactus2,
        Cactus3,
        Pterodactyl
    }

    private Type _type = Type.Cactus1;

    public Rectangle Bounds { get; private set; }

    private Obstacle(Rectangle bounds, Type type)
    {
        Bounds = bounds;
        _type = type;
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

    public void Move(GameState gameState) => Bounds.Translate(-gameState.Speed, 0);

    public void LayoutUpdated(LayoutData layout) =>
        Bounds.SetPosition(Bounds.X, layout.Viewport.Height - LayoutConstants.BottomMargin - Bounds.Height);

    public static IGameEntity Create(GameState state) => ObstacleGenerator.Create(state);

    internal static Obstacle Create(Rectangle bounds, Type type) => new(bounds, type);
}
