using System.Drawing;

namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class DinoPlayer
{
    internal enum States
    {
        Running,
        Jumping,
        Crouching
    }

    private int _jumpSpeed = 10;
    private States _state;
    private Rectangle _hitBox;

    public int Bottom { get; private set; } = 20;

    public Rectangle Location { get; }

    public DinoPlayer(GameState state)
    {
        Location = new(
            state.Layout.Dino.Width,
            state.Layout.Viewport.Height - LayoutConstants.BottomMargin - state.Layout.Dino.Height,
            state.Layout.Dino.Width,
            state.Layout.Dino.Height);

        _hitBox = Rectangle.Inflate(Location, -(Location.Width / 4), -5);
    }

    public string GetImage() => (_state == States.Crouching) ? "assets/trex/dino-crouch.gif" : "assets/trex/dino-run.gif";

    public void Jump()
    {
        if (_state == States.Running) _state = States.Jumping;
        if (_state == States.Crouching) _state = States.Running;
    }

    public void Crouch()
    {
        if (_state == States.Jumping) _state = States.Running;
        if (_state == States.Running) _state = States.Crouching;
    }

    public void Move()
    {
        if (_state == States.Jumping)
        {
            Bottom += _jumpSpeed;

            if (Bottom > 150)
            {
                _jumpSpeed = -10;
            }
            if (Bottom <= 20)
            {
                _state = States.Running;
                Bottom = 20;
                _jumpSpeed = 10;
            }
        }
    }
}
