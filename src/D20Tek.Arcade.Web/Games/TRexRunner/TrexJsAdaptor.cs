using Microsoft.JSInterop;

namespace D20Tek.Arcade.Web.Games.TRexRunner;

internal class TrexJsAdaptor
{
    private IJSRuntime _jsRuntime;
    private Action<string> _keydownAction;
    private Action<int> _updateWidthAction;
    private IJSObjectReference? _jsModule;
    private DotNetObjectReference<TrexJsAdaptor>? _dotNetRef;

    private TrexJsAdaptor(IJSRuntime jsRuntime, Action<string> keydownAction, Action<int> updateWidthAction)
    {
        _jsRuntime = jsRuntime;
        _keydownAction = keydownAction;
        _updateWidthAction = updateWidthAction;
    }

    public static async Task<TrexJsAdaptor> Create(IJSRuntime jsRuntime, Action<string> keydownAction, Action<int> updateWidthAction) =>
        await new TrexJsAdaptor(jsRuntime, keydownAction, updateWidthAction).RegisterJSCallbacks();

    private async Task<TrexJsAdaptor> RegisterJSCallbacks()
    {
        _dotNetRef = DotNetObjectReference.Create(this);
        await _jsRuntime.InvokeVoidAsync("addKeyListener", _dotNetRef);
        await _jsRuntime.InvokeVoidAsync("gameResizeHandler.init", _dotNetRef);

        return this;
    }

    [JSInvokable]
    public void HandleKeydown(string key) => _keydownAction(key);

    [JSInvokable]
    public void UpdateGameWidth(int newWidth) => _updateWidthAction(newWidth);

    public async Task<int> GetInitialGameWidth() =>
        await _jsRuntime.InvokeAsync<int>("getGameContainerWidth", ".game-container");

    public async ValueTask DisposeAsync()
    {
        if (_jsModule != null)
        {
            await _jsModule.InvokeVoidAsync("gameResizeHandler.dispose");
            await _jsModule.DisposeAsync();
        }
        _dotNetRef?.Dispose();
    }
}
