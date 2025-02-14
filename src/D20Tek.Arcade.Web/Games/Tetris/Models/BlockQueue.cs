namespace D20Tek.Arcade.Web.Games.Tetris.Models;

internal class BlockQueue
{
    private const int _numBlockTypes = 7;
    private readonly Random _random = new Random();

    public Block NextBlock { get; private set; }

    public BlockQueue()
    {
        NextBlock = RandomBlock();
    }

    private Block RandomBlock() => BlockFactory.Create(_random.Next(_numBlockTypes - 1) + 1);

    public Block GetAndUpdate()
    {
        var block = NextBlock;
        NextBlock = RandomBlock();

        return block;
    }
}
