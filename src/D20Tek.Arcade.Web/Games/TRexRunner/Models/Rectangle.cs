namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class Rectangle
{
    public static readonly Rectangle Empty = new(0, 0, 0, 0);

    public int X { get; private set; }

    public int Y { get; private set; }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public Rectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public Rectangle(Rectangle other)
    {
        X = other.X;
        Y = other.Y;
        Width = other.Width;
        Height = other.Height;
    }

    public void Translate(int x, int y)
    {
        X += x;
        Y += y;
    }

    public void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool IntersectsWith(Rectangle rect) =>
        (rect.X < X + Width) && (X < rect.X + rect.Width) &&
        (rect.Y < Y + Height) && (Y < rect.Y + rect.Height);

    public static Rectangle Inflate(Rectangle rect, int width, int height)
    {
        var x = rect.X - width;
        var y = rect.Y - height;

        var w = rect.Width + 2 * width;
        var h = rect.Height + 2 * height;

        return new(x, y, w, h);
    }
}
