namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal struct Size
{
    public int Width { get; }

    public int Height { get; }

    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public Size() { }
}
