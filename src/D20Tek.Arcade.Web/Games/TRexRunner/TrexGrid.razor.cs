using D20Tek.Arcade.Web.Common;
using D20Tek.Arcade.Web.Games.TRexRunner.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace D20Tek.Arcade.Web.Games.TRexRunner;

public partial class TrexGrid
{
    private const int _endGameDelay = 300;
    private TrexGameEngine _engine;

    public TrexGrid() => _engine = TrexGameEngine.Create(GameState.Create(new RandomRoller()), OnLevelChanged);

    [Parameter]
    public EventCallback<int> GameEnded { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await RegisterJSCallbacks();
            await GetInitialGameWidth();
            await RunGame();
        }
    }

    [JSInvokable]
    public void HandleKeydown(string key) => _engine.Input.ProcessKey(key);

    [JSInvokable]
    public void UpdateGameWidth(int newWidth) => _engine.UpdateLayout(newWidth);

    private async Task RegisterJSCallbacks()
    {
        var dotNetRef = DotNetObjectReference.Create(this);
        await JS.InvokeVoidAsync("addKeyListener", dotNetRef);
        await JS.InvokeVoidAsync("gameResizeHandler.init", dotNetRef);
    }

    private async Task GetInitialGameWidth() =>
        _engine.UpdateLayout(await JS.InvokeAsync<int>("getGameContainerWidth", ".game-container"));

    private async Task RunGame()
    {
        await _engine.GameLoop(StateHasChanged);
        await Task.Delay(_endGameDelay);
        await GameEnded.InvokeAsync(_engine.Score);
    }

    private Task OnLevelChanged(int newLevel)
    {
        return Task.CompletedTask;
    }
}
