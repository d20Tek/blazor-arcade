namespace D20Tek.Arcade.Web.Games.Snake;

internal static class GridValueMapper
{
    private static readonly Dictionary<GridValue, string> _gridValToImage = new()
    {
        { GridValue.Empty, Images.Empty },
        { GridValue.Snake, Images.Body },
        { GridValue.Food, Images.Food },
    };

    public static string GetImage(GridValue gridValue) => _gridValToImage[gridValue];
}
