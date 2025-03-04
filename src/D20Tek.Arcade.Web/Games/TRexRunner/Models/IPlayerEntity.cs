namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal interface IPlayerEntity
{
    public void Crouch();

    public bool DetectCollision(IReadOnlyList<IGameEntity> obstacles);

    public void Jump();
}
