using D20Tek.Arcade.Web.Common;
using D20Tek.Arcade.Web.Games.TRexRunner.Models;
using Microsoft.AspNetCore.Components;

namespace D20Tek.Arcade.Web.Games.TRexRunner;

public partial class TrexGrid
{
    private const int _endGameDelay = 300;
    private TrexJsAdaptor? _jsAdaptor;
    private TrexGameEngine _engine;

    public TrexGrid() => _engine = TrexGameEngine.Create(GameState.Create(new RandomRoller()));

    [Parameter]
    public EventCallback<int> GameEnded { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsAdaptor = await TrexJsAdaptor.Create(JS, _engine.Input.ProcessKey, _engine.UpdateLayout);
            _engine.UpdateLayout(await _jsAdaptor.GetInitialGameWidth());
            await RunGame();
        }
    }

    public void HandleKeydown(string key) => _engine.Input.ProcessKey(key);

    private async Task RunGame()
    {
        await _engine.GameLoop(StateHasChanged);
        await Task.Delay(_endGameDelay);
        await GameEnded.InvokeAsync(_engine.Score);
    }
}
