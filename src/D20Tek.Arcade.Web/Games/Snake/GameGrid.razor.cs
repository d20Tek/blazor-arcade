using D20Tek.Arcade.Web.Games.Snake.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace D20Tek.Arcade.Web.Games.Snake;

public partial class GameGrid
{
    private SnakeGameEngine? _engine;
    private string? _levelText;
    private bool _hasLevelText => !string.IsNullOrEmpty(_levelText);

    [Parameter]
    public int Rows { get; set; }

    [Parameter]
    public int Columns { get; set; }

    [Parameter]
    public EventCallback<int> GameEnded { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var dotNetRef = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("addKeyListener", dotNetRef);

            _engine = new SnakeGameEngine(Rows, Columns, StateHasChanged, OnLevelChanged);
            await _engine.RunGameAsync();
            await GameEnded.InvokeAsync(_engine.GetScore());
        }
    }

    [JSInvokable]
    public void HandleKeydown(string key)
    {
        if (key == "ArrowUp") _engine?.ChangeDirection(Direction.Up);
        if (key == "ArrowDown") _engine?.ChangeDirection(Direction.Down);
        if (key == "ArrowLeft") _engine?.ChangeDirection(Direction.Left);
        if (key == "ArrowRight") _engine?.ChangeDirection(Direction.Right);
    }

    private string? GetCellStyle(int row, int col)
    {
        if (_engine is null) return null;

        var cell = _engine.GetGridCell(row, col);
        return !string.IsNullOrEmpty(cell.ImageUrl)
                    ? $"background-image: url('{cell.ImageUrl}'); background-size: cover; transform: rotate({cell.Rotation}deg)"
                    : null;
    }

    private async Task OnLevelChanged(int newLevel)
    {
        _levelText = $"CONGRATS! NOW STARTING LEVEL {newLevel} ...";
        StateHasChanged();
        await Task.Delay(2000);
        _levelText = null;
    }
}
