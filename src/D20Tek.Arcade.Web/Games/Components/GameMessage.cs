namespace D20Tek.Arcade.Web.Games.Components;

public class GameMessage
{
    public string GameTitle { get; set; } = string.Empty;

    public string GameImageUrl { get; set; } = string.Empty;
    
    public string Score { get; set; } = string.Empty;

    public bool HasImageUrl => !string.IsNullOrEmpty(GameImageUrl);
};
