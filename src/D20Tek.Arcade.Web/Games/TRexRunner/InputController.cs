using D20Tek.Arcade.Web.Games.Components;

namespace D20Tek.Arcade.Web.Games.TRexRunner;

internal class InputController
{
    private readonly TrexGameEngine _engine;

    private InputController(TrexGameEngine engine) => _engine = engine;

    public static InputController Create(TrexGameEngine engine) => new(engine);

    public async Task ProcessKey(string key)
    {
        if (key == KnownKeys.Q || key == KnownKeys.q) await _engine.EndGame();
        if (key == KnownKeys.ArrowUp) _engine.Dino.Jump();
        if (key == KnownKeys.ArrowDown) _engine.Dino.Crouch();
    }
}
