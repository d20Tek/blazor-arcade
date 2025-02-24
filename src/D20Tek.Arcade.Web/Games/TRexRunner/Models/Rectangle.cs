﻿namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class Rectangle
{
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

    public void Translate(int x, int y)
    {
        X += x;
        Y += y;
    }

    public static Rectangle Inflate(Rectangle rect, int width, int height)
    {
        var x = rect.X - width;
        var y = rect.Y - height;

        var w = rect.Width + 2 * width;
        var h = rect.Height + 2 * height;

        return new(x, y, w, h);
    }
}
