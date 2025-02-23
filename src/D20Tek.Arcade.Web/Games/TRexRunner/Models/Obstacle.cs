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

    public int Left { get; private set; }

    public Obstacle(int startingLeft, int type)
    {
        Left = startingLeft;
        _type = (Type)type;
    }

    public string GetImage() =>
        _type switch
        {
            Type.Cactus1 => "assets/trex/cactus1.png",
            Type.Cactus2 => "assets/trex/cactus2.png",
            Type.Cactus3 => "assets/trex/cactus2.png",
            Type.Pterodactyl => "assets/trex/pteryl-flying.gif",
            _ => throw new NotImplementedException(),
        };

    public void Move()
    {
        Left -= _speed;
    }
}
