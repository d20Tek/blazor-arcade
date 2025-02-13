namespace D20Tek.Arcade.Web.Games.Tetris.Models;

internal static class BlockFactory
{
    public record Template(int Id, Position[][] Tiles, Position StartOffset);

    public static Template[] _blocks =
    [
        // IBlock definition
        new(
            1,
            [
                [new(1, 0), new(1, 1), new(1, 2), new(1, 3)],
                [new(0, 2), new(1, 2), new(2, 2), new(3, 2)],
                [new(2, 0), new(2, 1), new(2, 2), new(2, 3)],
                [new(0, 1), new(1, 1), new(2, 1), new(3, 1)]
            ],
            new(-1, 3)),
        // JBlock definition
        new(
            2,
            [
                [new(0, 0), new(1, 0), new(1, 1), new(1, 2)],
                [new(0, 1), new(0, 2), new(1, 1), new(2, 1)],
                [new(1, 0), new(1, 1), new(1, 2), new(2, 2)],
                [new(0, 1), new(1, 1), new(2, 0), new(2, 1)]
            ],
            new(0, 3)),
        // LBlock definition
        new(
            3,
            [
                [new(0, 2), new(1, 0), new(1, 1), new(1, 2)],
                [new(0, 1), new(1, 1), new(2, 1), new(2, 2)],
                [new(1, 0), new(1, 1), new(1, 2), new(2, 0)],
                [new(0, 0), new(0, 1), new(1, 1), new(2, 1)]
            ],
            new(0, 3)),
        // OBlock definition
        new(
            4,
            [ [new(0, 0), new(0, 1), new(1, 0), new(1, 1)] ],
            new(0, 4)),
        // SBlock definition
        new(
            5,
            [
                [new(0, 1), new(0, 2), new(1, 0), new(1, 1)],
                [new(0, 1), new(1, 1), new(1, 2), new(2, 2)],
                [new(1, 1), new(1, 2), new(2, 0), new(2, 1)],
                [new(0, 0), new(1, 0), new(1, 1), new(2, 1)]
            ],
            new(0, 3)),
        // TBlock definition
        new(
            6,
            [
                [new(0, 1), new(1, 0), new(1, 1), new(1, 2)],
                [new(0, 1), new(1, 1), new(1, 2), new(2, 1)],
                [new(1, 0), new(1, 1), new(1, 2), new(2, 1)],
                [new(0, 1), new(1, 0), new(1, 1), new(2, 1)]
            ],
            new(0, 3)),
        // ZBlock definition
        new(
            7,
            [
                [new(0, 0), new(0, 1), new(1, 1), new(1, 2)],
                [new(0, 2), new(1, 1), new(1, 2), new(2, 1)],
                [new(1, 0), new(1, 1), new(2, 1), new(2, 2)],
                [new(0, 1), new(1, 0), new(1, 1), new(2, 0)]
            ],
            new(0, 3)),
    ];

    public static Block Create(int blockId)
    {
        var template = _blocks[blockId];
        return new(template.Id, template.Tiles, template.StartOffset);
    }
}

