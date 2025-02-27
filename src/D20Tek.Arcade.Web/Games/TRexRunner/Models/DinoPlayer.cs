namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class DinoPlayer : IGameEntity, IPlayerEntity
{
    internal enum States
    {
        Running,
        Jumping,
        Crouching,
        Dead
    }

    private int _jumpSpeed = 10;
    private int _bottom = 20;
    private States _state;
    private BoundingBox _boundingBox;

    public Rectangle Bounds => _boundingBox.Bounds;

    private DinoPlayer(GameState state)
    {
        _boundingBox = new(state.Layout);
    }

    public static DinoPlayer Create(GameState state) => new(state);

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
            Reset(false);
            _state = States.Crouching;
        }
    }

    public void Move()
    {
        if (_state == States.Jumping)
        {
            _bottom += _jumpSpeed;
            _boundingBox.Translate(0, -_jumpSpeed);

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

    public bool DetectCollision(IReadOnlyList<IGameEntity> obstacles)
    {
        var obstacle = obstacles.FirstOrDefault();
        if (obstacle is null) return false;

        var collided = _boundingBox.IntersectsWith(obstacle.Bounds);
        if (collided is true) _state = States.Dead;

        return collided;
    }

    private void Reset(bool useOriginal = true)
    {
        if (useOriginal)
            _boundingBox.SetOriginal();
        else
            _boundingBox.SetCrouch();

        _jumpSpeed = 10;
        _bottom = 20;
    }
}
