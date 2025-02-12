using D20Tek.Arcade.Web.Games.Components;

namespace D20Tek.Arcade.Web.Games.Snake;

public partial class Index
{
    private GameStages _currentStage = GameStages.Start;
    private readonly GameMessage _message = new()
    {
        GameTitle = "THE SNAKE GAME",
        GameImageUrl = "assets/snake/snake-logo.png"
    };

    private void OnGameStarted() => _currentStage = GameStages.Running;

    private void OnGameEnded(int score)
    {
        _message.Score = score.ToString();
        _currentStage = GameStages.GameOver;
    }
}
