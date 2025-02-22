using D20Tek.Arcade.Web.Games.Components;
using Microsoft.JSInterop;

namespace D20Tek.Arcade.Web.Games.TRexRunner;

public partial class DinoPlayer
{
    private bool _isJumping = false;
    private string _dinoClass = "";

    public async Task Jump()
    {
        if (!_isJumping)
        {
            _isJumping = true;
            _dinoClass = "jump";
            StateHasChanged();

            await Task.Delay(600); // Duration of jump animation
            _dinoClass = "";
            _isJumping = false;
            StateHasChanged();
        }
    }

    public async Task DoubleJump()
    {
        if (!_isJumping)
        {
            _isJumping = true;
            _dinoClass = "double-jump";
            StateHasChanged();

            await Task.Delay(1000); // Duration of jump animation
            _dinoClass = "";
            _isJumping = false;
            StateHasChanged();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var dotNetRef = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("addKeyListener", dotNetRef);
        }
    }

    [JSInvokable]
    public async Task HandleKeydown(string key)
    {
        if (key == KnownKeys.ArrowUp) await Jump();
        if (key == KnownKeys.Space) await DoubleJump();
    }
}
