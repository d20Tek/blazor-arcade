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

    private string? _gameOverText = null;

    private bool ShowGameOverText => !string.IsNullOrEmpty(_gameOverText);

    private void ShowGameOver()
    {
        _gameOverText = "GAME OVER... WOULD YOU LIKE TO PLAY AGAIN?";
        StateHasChanged();
    }

    private void OnGameStarted() => _currentStage = Stages.Running;

    private void OnGameEnded() => _currentStage = Stages.GameOver;
}
