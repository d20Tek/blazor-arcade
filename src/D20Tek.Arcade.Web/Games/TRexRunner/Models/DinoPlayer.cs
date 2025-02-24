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
    private int _bottom = 20;

    public Rectangle Bounds { get; private set; }

    public DinoPlayer(GameState state)
    {
        Bounds = new(
            state.Layout.Dino.Width,
            state.Layout.Viewport.Height - LayoutConstants.BottomMargin - state.Layout.Dino.Height,
            state.Layout.Dino.Width,
            state.Layout.Dino.Height);

        _hitBox = Rectangle.Inflate(Bounds, -(Bounds.Width / 4), -5);
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
            _bottom += _jumpSpeed;
            Bounds.Translate(0, -_jumpSpeed);

            if (_bottom > 150)
            {
                _jumpSpeed = -10;
            }
            if (_bottom <= 20)
            {
                _state = States.Running;
                _bottom = 20;
                _jumpSpeed = 10;
            }
        }
    }
}
