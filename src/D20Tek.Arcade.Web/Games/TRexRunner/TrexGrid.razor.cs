using D20Tek.Arcade.Web.Common;
using D20Tek.Arcade.Web.Games.TRexRunner.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace D20Tek.Arcade.Web.Games.TRexRunner;

public partial class TrexGrid
{
    private const int _endGameDelay = 300;

    private GameState _state;
    private TrexGameEngine _engine;

    public TrexGrid()
    {
        _state = GameState.Create(new RandomRoller());
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
            await Task.Delay(_endGameDelay);
            await GameEnded.InvokeAsync(_engine.Score);
        }
    }

    [JSInvokable]
    public async Task HandleKeydown(string key) => await _engine.Input.ProcessKey(key);
}
