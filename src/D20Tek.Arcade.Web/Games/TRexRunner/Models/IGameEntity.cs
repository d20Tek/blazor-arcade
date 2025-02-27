namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal interface IGameEntity
{
    public Rectangle Bounds { get; }

    public string GetImage();

    public void LayoutUpdated(LayoutData layout);

    public void Move();
}
