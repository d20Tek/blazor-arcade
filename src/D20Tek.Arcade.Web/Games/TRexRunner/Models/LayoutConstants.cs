namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal static class LayoutConstants
{
    private static readonly LayoutData[] _layouts =
    [
        new LayoutData
        {
            Viewport = new(400, 300),
            Dino = new(40, 40),
            Crouched = new(40, 26),
            Obstacles = [new(16, 30), new(30, 30), new(36, 30), new(33, 16)]
        },
        new LayoutData
        {
            Viewport = new(480, 360),
            Dino = new(52, 52),
            Crouched = new(52, 32),
            Obstacles = [new(18, 35), new(34, 35), new(40, 35), new(37, 20)]
        },
        new LayoutData
        {
            Viewport = new(600, 480),
            Dino = new(64, 64),
            Crouched = new(64, 42),
            Obstacles = [new(20, 40), new(38, 40), new(46, 40), new(34, 24)]
        }
    ];

    public const int BottomMargin = 20;

    public static LayoutData GetLayout(LayoutSize layout) => _layouts[(int)layout];

    public static LayoutSize LayoutSizeFromWidth(int width) =>
        width switch
        {
            <= 400 => LayoutSize.Small,
            >= 597 => LayoutSize.Large,
            _ => LayoutSize.Medium
        };
}
