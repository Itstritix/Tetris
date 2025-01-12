using System.Collections.Generic;

namespace Tetris.Blocks
{
    // Abstract class representing a generic Tetris block.
    public abstract class Block
    {
        // Array of relative positions for each rotation state of the block.
        protected abstract Position[][] Tiles { get; }

        // Default starting offset of the block on the grid.
        protected abstract Position StartOffset { get; }

        // Unique identifier for the block type.
        public abstract int Id { get; }

        private int rotationState; // Current rotation state of the block.
        private Position offset;   // Current offset position of the block on the grid.

        // Constructor initializing the block's position to the starting offset.
        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        // Returns the absolute positions of the block's tiles in the grid.
        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        // Rotates the block clockwise.
        public void RotateCW()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        // Rotates the block counterclockwise.
        public void RotateCCW()
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        // Moves the block by a specified number of rows and columns.
        public void Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        // Resets the block to its initial position and rotation state.
        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}
