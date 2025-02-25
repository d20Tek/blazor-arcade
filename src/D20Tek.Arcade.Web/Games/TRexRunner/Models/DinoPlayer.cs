namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class DinoPlayer
{
    internal enum States
    {
        Running,
        Jumping,
        Crouching,
        Dead
    }

    private int _jumpSpeed = 10;
    private States _state;
    private Rectangle _resetBounds;
    private Rectangle _crouchedBounds;
    private Rectangle _hitBox;
    private int _bottom = 20;

    public Rectangle Bounds { get; private set; }

    public DinoPlayer(GameState state)
    {
        _resetBounds = new(
            state.Layout.Dino.Width,
            state.Layout.Viewport.Height - LayoutConstants.BottomMargin - state.Layout.Dino.Height,
            state.Layout.Dino.Width,
            state.Layout.Dino.Height);

        _crouchedBounds = new(
            state.Layout.Crouched.Width,
            state.Layout.Viewport.Height - LayoutConstants.BottomMargin - state.Layout.Crouched.Height,
            state.Layout.Crouched.Width,
            state.Layout.Crouched.Height);

        Bounds = new(_resetBounds);

        _hitBox = Rectangle.Inflate(Bounds, -(Bounds.Width / 4), -5);
    }

    public string GetImage() =>
        _state switch
        {
            States.Running => "assets/trex/dino-run.gif",
            States.Jumping => "assets/trex/dino-run.gif",
            States.Crouching => "assets/trex/dino-crouch.gif",
            States.Dead => "assets/trex/dino-dead.png",
            _ => throw new InvalidOperationException(),
        };
        
    public void Jump()
    {
        if (_state == States.Crouching)
        {
            Reset();
            _state = States.Running;
        }
        else if (_state == States.Running)
        {
            _state = States.Jumping;
        }
    }

    public void Crouch()
    {
        if (_state == States.Jumping)
        {
            Reset();
            _state = States.Running;
        }
        else if (_state == States.Running)
        {
            Bounds = new(_crouchedBounds);
            _hitBox = Rectangle.Inflate(Bounds, -(Bounds.Width / 4), -5);
            _state = States.Crouching;
        }
    }

    public void Move()
    {
        if (_state == States.Jumping)
        {
            _bottom += _jumpSpeed;
            Bounds.Translate(0, -_jumpSpeed);
            _hitBox.Translate(0, -_jumpSpeed);

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

    public bool DetectCollision(Obstacles obstacles)
    {
        var obstacle = obstacles.First();
        if (obstacle is null) return false;

        var collided = _hitBox.IntersectsWith(obstacle.Bounds);
        if (collided is true) _state = States.Dead;

        return collided;
    }

    private void Reset()
    {
        Bounds = new(_resetBounds);
        _hitBox = Rectangle.Inflate(Bounds, -(Bounds.Width / 4), -5);
        _jumpSpeed = 10;
        _bottom = 20;
    }
}
