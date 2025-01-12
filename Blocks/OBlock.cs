namespace Tetris.Blocks
{
    // Class representing the "O" Tetris block (square shape).
    public class OBlock : Block
    {
        // Define the tiles' relative positions (fixed shape, no rotation).
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(0, 0), new(0, 1), new(1, 0), new(1, 1) }
        };

        public override int Id => 4; // Unique identifier for the "O" block.

        // Starting offset of the "O" block in the grid.
        protected override Position StartOffset => new Position(0, 4);

        // Property to retrieve the block's tile positions.
        protected override Position[][] Tiles => tiles;
    }
}
