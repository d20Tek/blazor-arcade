using D20Tek.Arcade.Web.Games.Components;

namespace D20Tek.Arcade.Web.Games.Snake;

public partial class Index
{
    enum Stages
    {
        Start,
        Running,
        GameOver,
        Exit
    }

    private Stages _currentStage = Stages.Start;
    private GameMessage _message = new() { GameTitle = "THE SNAKE GAME" };

    private void OnGameStarted() => _currentStage = Stages.Running;

    private void OnGameEnded(int score)
    {
        _message.Score = score.ToString();
        _currentStage = Stages.GameOver;
    }
}
