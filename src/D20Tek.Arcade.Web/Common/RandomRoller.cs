namespace D20Tek.Arcade.Web.Common;

public class RandomRoller : IRandomRoller
{
    private readonly Random _rnd;

    public RandomRoller() => _rnd = new();

    public RandomRoller(int seed) => _rnd = new(seed);

    public int Next(int minValue, int maxValue) => _rnd.Next(minValue, maxValue);
}
