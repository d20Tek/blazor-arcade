namespace D20Tek.Arcade.Web.Games.Tetris.Models;

internal static class BlockFactory
{
    public record Template(int Id, Position[][] Tiles, Position StartOffset, string BlockImage, string TileImage);

    public static Template[] _blocks =
    [
        // Empty
        new(0, [], new(0, 0), "", ""),
        // IBlock definition
        new(
            1,
            [
                [new(1, 0), new(1, 1), new(1, 2), new(1, 3)],
                [new(0, 2), new(1, 2), new(2, 2), new(3, 2)],
                [new(2, 0), new(2, 1), new(2, 2), new(2, 3)],
                [new(0, 1), new(1, 1), new(2, 1), new(3, 1)]
            ],
            new(-1, 3),
            "assets/tetris/blocki.png",
            "assets/tetris/tile-cyan.png"),
        // JBlock definition
        new(
            2,
            [
                [new(0, 0), new(1, 0), new(1, 1), new(1, 2)],
                [new(0, 1), new(0, 2), new(1, 1), new(2, 1)],
                [new(1, 0), new(1, 1), new(1, 2), new(2, 2)],
                [new(0, 1), new(1, 1), new(2, 0), new(2, 1)]
            ],
            new(0, 3),
            "assets/tetris/blockj.png",
            "assets/tetris/tile-blue.png"),
        // LBlock definition
        new(
            3,
            [
                [new(0, 2), new(1, 0), new(1, 1), new(1, 2)],
                [new(0, 1), new(1, 1), new(2, 1), new(2, 2)],
                [new(1, 0), new(1, 1), new(1, 2), new(2, 0)],
                [new(0, 0), new(0, 1), new(1, 1), new(2, 1)]
            ],
            new(0, 3),
            "assets/tetris/blockl.png",
            "assets/tetris/tile-orange.png"),
        // OBlock definition
        new(
            4,
            [ [new(0, 0), new(0, 1), new(1, 0), new(1, 1)] ],
            new(0, 4),
            "assets/tetris/blocko.png",
            "assets/tetris/tile-yellow.png"),
        // SBlock definition
        new(
            5,
            [
                [new(0, 1), new(0, 2), new(1, 0), new(1, 1)],
                [new(0, 1), new(1, 1), new(1, 2), new(2, 2)],
                [new(1, 1), new(1, 2), new(2, 0), new(2, 1)],
                [new(0, 0), new(1, 0), new(1, 1), new(2, 1)]
            ],
            new(0, 3),
            "assets/tetris/blocks.png",
            "assets/tetris/tile-green.png"),

        // TBlock definition
        new(
            6,
            [
                [new(0, 1), new(1, 0), new(1, 1), new(1, 2)],
                [new(0, 1), new(1, 1), new(1, 2), new(2, 1)],
                [new(1, 0), new(1, 1), new(1, 2), new(2, 1)],
                [new(0, 1), new(1, 0), new(1, 1), new(2, 1)]
            ],
            new(0, 3),
            "assets/tetris/blockt.png",
            "assets/tetris/tile-purple.png"),
        // ZBlock definition
        new(
            7,
            [
                [new(0, 0), new(0, 1), new(1, 1), new(1, 2)],
                [new(0, 2), new(1, 1), new(1, 2), new(2, 1)],
                [new(1, 0), new(1, 1), new(2, 1), new(2, 2)],
                [new(0, 1), new(1, 0), new(1, 1), new(2, 0)]
            ],
            new(0, 3),
            "assets/tetris/blockz.png",
            "assets/tetris/tile-red.png"),
    ];

    public static Block Create(int blockId)
    {
        var template = _blocks[blockId];
        return new(template.Id, template.Tiles, template.StartOffset);
    }

    public static string GetBlockImage(int blockId) => _blocks[blockId].BlockImage;

    public static string GetTileImage(int blockId) => _blocks[blockId].TileImage;
}

