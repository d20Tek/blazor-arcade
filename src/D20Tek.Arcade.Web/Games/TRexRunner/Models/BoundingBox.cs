namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class BoundingBox
{
    private Rectangle _resetBounds;
    private Rectangle _crouchedBounds;
    private Rectangle _hitBox;

    public Rectangle Bounds { get; private set; }

    public BoundingBox(LayoutData layoutData)
    {
        _resetBounds = new(
            layoutData.Dino.Width,
            layoutData.Viewport.Height - LayoutConstants.BottomMargin - layoutData.Dino.Height,
            layoutData.Dino.Width,
            layoutData.Dino.Height);

        _crouchedBounds = new(
            layoutData.Crouched.Width,
            layoutData.Viewport.Height - LayoutConstants.BottomMargin - layoutData.Crouched.Height,
            layoutData.Crouched.Width,
            layoutData.Crouched.Height);

        Bounds = new(_resetBounds);
        _hitBox = CaclulateHitBox();
    }

    public bool IntersectsWith(Rectangle other) => _hitBox.IntersectsWith(other);

    public void SetOriginal()
    {
        Bounds = new(_resetBounds);
        _hitBox = CaclulateHitBox();
    }

    public void SetCrouch()
    {
        Bounds = new(_crouchedBounds);
        _hitBox = CaclulateHitBox();
    }

    public void Translate(int x, int y)
    {
        Bounds.Translate(x, y);
        _hitBox.Translate(x, y);
    }

    private Rectangle CaclulateHitBox() => Rectangle.Inflate(Bounds, -(Bounds.Width / 4), -5);
}
