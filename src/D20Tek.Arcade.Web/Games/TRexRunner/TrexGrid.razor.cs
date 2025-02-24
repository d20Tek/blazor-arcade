using D20Tek.Arcade.Web.Games.Components;
using D20Tek.Arcade.Web.Games.TRexRunner.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace D20Tek.Arcade.Web.Games.TRexRunner;

public partial class TrexGrid
{
    private GameState _state;
    private TrexGameEngine _engine;

    public TrexGrid()
    {
        _state = new();
        _engine = new(_state);
    }

    [Parameter]
    public EventCallback<int> GameEnded { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var dotNetRef = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("addKeyListener", dotNetRef);

            await _engine.GameLoop(StateHasChanged);
            await GameEnded.InvokeAsync(_engine.Score);
        }
    }

    [JSInvokable]
    public async Task HandleKeydown(string key)
    {
        if (key == "q") await _engine.EndGame();
        if (key == KnownKeys.ArrowUp) _engine.Dino.Jump();
        if (key == KnownKeys.ArrowDown) _engine.Dino.Crouch();
    }
}
