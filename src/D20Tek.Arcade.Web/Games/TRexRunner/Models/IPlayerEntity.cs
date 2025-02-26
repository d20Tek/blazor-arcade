namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal interface IPlayerEntity
{
    public bool DetectCollision(IReadOnlyList<IGameEntity> obstacles);

    public void Crouch();

    public void Jump();
}
