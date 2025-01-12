namespace Tetris.Blocks
{
    // Class representing the "T" Tetris block (T-shape).
    public class TBlock : Block
    {
        public override int Id => 6; // Unique identifier for the "T" block.

        // Starting offset of the "T" block in the grid.
        protected override Position StartOffset => new(0, 3);

        // Define the tiles' relative positions for each rotation state.
        protected override Position[][] Tiles => new Position[][]
        {
            new Position[] { new(0, 1), new(1, 0), new(1, 1), new(1, 2) },
            new Position[] { new(0, 1), new(1, 1), new(1, 2), new(2, 1) },
            new Position[] { new(1, 0), new(1, 1), new(1, 2), new(2, 1) },
            new Position[] { new(0, 1), new(1, 0), new(1, 1), new(2, 1) }
        };
    }
}
