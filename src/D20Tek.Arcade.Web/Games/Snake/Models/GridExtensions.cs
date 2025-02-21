﻿using D20Tek.Arcade.Web.Games.Snake.Model;

namespace D20Tek.Arcade.Web.Games.Snake.Models;

internal static class GridExtensions
{
    private static readonly Random _random = new();

    private static IEnumerable<Position> EmptyPositions(this GridValue[,] grid, int rows, int cols)
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (grid[r, c] == GridValue.Empty)
                {
                    yield return new Position(r, c);
                }
            }
        }
    }

    public static void AddFood(this GridValue[,] grid, int rows, int cols, int amount) =>
        grid.AddItem(rows, cols, amount, GridValue.Food);

    public static void AddRock(this GridValue[,] grid, int rows, int cols, int amount) =>
        grid.AddItem(rows, cols, amount, GridValue.Rock);

    private static void AddItem(this GridValue[,] grid, int rows, int cols, int amount, GridValue itemType)
    {
        var empty = grid.EmptyPositions(rows, cols).ToList();

        for (int a = 1; a <= amount; a++)
        {
            var pos = empty[_random.Next(empty.Count)];
            grid[pos.Row, pos.Col] = itemType;

            empty.Remove(pos);
        }
    }
}
