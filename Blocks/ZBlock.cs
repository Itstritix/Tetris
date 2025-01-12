namespace Tetris.Blocks
{
    // Class representing the "Z" Tetris block (reverse zigzag shape).
    public class ZBlock : Block
    {
        public override int Id => 7; // Unique identifier for the "Z" block.

        // Starting offset of the "Z" block in the grid.
        protected override Position StartOffset => new(0, 3);

        // Define the tiles' relative positions for each rotation state.
        protected override Position[][] Tiles => new Position[][]
        {
            new Position[] { new(0, 0), new(0, 1), new(1, 1), new(1, 2) },
            new Position[] { new(0, 2), new(1, 1), new(1, 2), new(2, 1) },
            new Position[] { new(1, 0), new(1, 1), new(2, 1), new(2, 2) },
            new Position[] { new(0, 1), new(1, 0), new(1, 1), new(2, 0) }
        };
    }
}
