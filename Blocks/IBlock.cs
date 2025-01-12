namespace Tetris.Blocks
{
    // Class representing the "I" Tetris block (long straight shape).
    public class IBlock : Block
    {
        // Define the tiles' relative positions for each rotation state.
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(1, 0), new(1, 1), new(1, 2), new(1, 3) },
            new Position[] { new(0, 2), new(1, 2), new(2, 2), new(3, 2) },
            new Position[] { new(2, 0), new(2, 1), new(2, 2), new(2, 3) },
            new Position[] { new(0, 1), new(1, 1), new(2, 1), new(3, 1) }
        };

        public override int Id => 1; // Unique identifier for the "I" block.

        // Starting offset of the "I" block in the grid.
        protected override Position StartOffset => new Position(-1, 3);

        // Property to retrieve the block's tile positions.
        protected override Position[][] Tiles => tiles;
    }
}
