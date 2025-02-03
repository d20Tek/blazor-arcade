namespace D20Tek.Arcade.Web.Games.Snake;

public partial class Index
{
    private string? _countDownText = null;
    private string? _gameOverText = null;

    private bool ShowCountDownText => !string.IsNullOrEmpty(_countDownText);
    private bool ShowGameOverText => !string.IsNullOrEmpty(_gameOverText);

    private async Task ShowCountdown()
    {
        for (int i = 3; i >= 1; i--)
        {
            _countDownText = $"Game starts in ... {i}";
            StateHasChanged() ;

            await Task.Delay(500);
        }

        _countDownText = null;
    }

    private async Task ShowGameOver()
    {
        _gameOverText = "GAME OVER... WOULD YOU LIKE TO PLAY AGAIN?";
        StateHasChanged();
    }
}
