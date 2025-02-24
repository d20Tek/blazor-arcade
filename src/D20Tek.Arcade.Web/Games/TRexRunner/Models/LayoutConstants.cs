namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal static class LayoutConstants
{
    private static readonly LayoutData[] _layouts =
    [
        new LayoutData
        {
            Viewport = new(400, 300),
            Dino = new(40, 40)
        },
        new LayoutData
        {
            Viewport = new(480, 360),
            Dino = new(52, 52)
        },
        new LayoutData
        {
            Viewport = new(600, 480),
            Dino = new(64, 64)
        }
    ];

    public const int BottomMargin = 20;

    public static LayoutData GetLayout(LayoutSize layout) => _layouts[(int)layout];
}
