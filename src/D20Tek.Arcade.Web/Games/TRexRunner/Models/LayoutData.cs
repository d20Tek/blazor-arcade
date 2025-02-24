﻿using System.Drawing;

namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal enum LayoutSize
{
    Small,
    Medium,
    Large
}

internal class LayoutData
{
    public Size Viewport { get; init; }

    public Size Dino { get; init; }
}
