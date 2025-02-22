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

    public int Bottom { get; private set; } = 20;

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

            if (Bottom > 170)
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
