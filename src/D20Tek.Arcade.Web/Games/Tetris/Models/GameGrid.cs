namespace D20Tek.Arcade.Web.Games.Tetris.Models;

internal class GameGrid
{
    private readonly int[,] _grid;

    public int Rows { get; }

    public int Columns { get; }

    public int this[int r, int c]
    {
        get => _grid[r, c];
        set => _grid[r, c] = value;
    }

    public GameGrid(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        _grid = new int[Rows, Columns];
    }

    public bool IsWithin(int r, int c) => r >= 0 && r < Rows && c >= 0 && c < Columns;

    public bool IsEmpty(int r, int c) => IsWithin(r, c) && _grid[r, c] == 0;

    public bool IsRowFull(int row) => Enumerable.Range(0, Columns).All(c => _grid[row, c] != 0);

    public bool IsRowEmpty(int row) => Enumerable.Range(0, Columns).All(c => _grid[row, c] == 0);

    public int ClearFullRows()
    {
        int cleared = 0;

        for (int r = Rows - 1; r >= 0; r--)
        {
            if (IsRowFull(r))
            {
                ClearRow(r);
                cleared++;
            }
            else if(cleared > 0)
            {
                MoveRowDown(r, cleared);
            }
        }

        return cleared;
    }

    private void ClearRow(int row)
    {
        for (var c = 0;  c < Columns; c++)
        {
            _grid[row, c] = 0;
        }
    }

    private void MoveRowDown(int r, int numRows)
    {
        for (var c = 0; c < Columns; c++)
        {
            _grid[r + numRows, c] = _grid[r, c];
            _grid[r, c] = 0;
        }
    }
}
