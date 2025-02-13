using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace D20Tek.Arcade.Web.Games.Tetris;

public partial class TetrisGrid
{
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
        }
    }

    [JSInvokable]
    public void HandleKeydown(string key)
    {
    }

    private string? GetCellStyle(int row, int col)
    {
        return null;
    }

    private async Task OnLevelChanged(int newLevel)
    {
        _levelText = $"MOVING TO LEVEL {newLevel} ...";
        StateHasChanged();
        await Task.Delay(300);
        _levelText = null;
    }
}
