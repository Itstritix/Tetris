namespace Tetris.Blocks
{
    // Class representing a position in the grid (row and column).
    public class Position
    {
        public int Row { get; set; }    // Row index of the position.
        public int Column { get; set; } // Column index of the position.

        // Constructor to initialize the position with row and column values.
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
