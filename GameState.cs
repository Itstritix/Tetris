using Tetris.Blocks;

namespace Tetris
{
    // Class representing the current state of the Tetris game.
    public class GameState
    {
        private Block currentBlock; // The currently active block in the game.

        // Property for accessing or setting the current block.
        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset(); // Reset the block to its initial state.

                // Move the block down slightly to ensure it fits initially.
                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);

                    // Undo the move if the block does not fit.
                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        public GameGrid GameGrid { get; } // The grid used for the game.
        public BlockQueue BlockQueue { get; } // Queue managing upcoming blocks.
        public bool GameOver { get; private set; } // Indicates whether the game is over.
        public int Score { get; private set; } // The player's current score.
        public Block HeldBlock { get; private set; } // Block currently held by the player.
        public bool CanHold { get; private set; } // Indicates if the player can hold a block.

        // Constructor initializing the game state with a grid and block queue.
        public GameState()
        {
            GameGrid = new GameGrid(22, 10); // Create a grid with 22 rows and 10 columns.
            BlockQueue = new BlockQueue(); // Initialize the block queue.
            CurrentBlock = BlockQueue.GetAndUpdate(); // Set the first block.
            CanHold = true; // Allow holding initially.
        }

        // Checks if the current block fits within the grid.
        private bool BlockFits()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                if (!GameGrid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
        }

        // Allows the player to hold the current block.
        public void HoldBlock()
        {
            if (!CanHold) return;

            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock; // Hold the current block.
                CurrentBlock = BlockQueue.GetAndUpdate(); // Get the next block.
            }
            else
            {
                // Swap the held block with the current block.
                Block temp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = temp;
            }

            CanHold = false; // Disable further holds until the block is placed.
        }

        // Rotates the current block clockwise.
        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();

            // Undo rotation if the block does not fit.
            if (!BlockFits())
            {
                CurrentBlock.RotateCCW();
            }
        }

        // Rotates the current block counterclockwise.
        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();

            // Undo rotation if the block does not fit.
            if (!BlockFits())
            {
                CurrentBlock.RotateCW();
            }
        }

        // Moves the current block one cell to the left.
        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);

            // Undo move if the block does not fit.
            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }

        // Moves the current block one cell to the right.
        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            // Undo move if the block does not fit.
            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        // Checks if the game is over (i.e., no space for new blocks).
        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        // Places the current block on the grid and handles game logic.
        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id; // Mark the grid with the block ID.
            }

            Score += GameGrid.ClearFullRows(); // Update the score by clearing full rows.

            // Check if the game is over or get a new block.
            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                CanHold = true; // Enable holding again.
            }
        }

        // Moves the current block one cell downward.
        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);

            // If the block cannot move further, place it on the grid.
            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0); // Undo the move.
                PlaceBlock();
            }
        }

        // Calculates the distance the block can drop straight down.
        private int TileDropDistance(Position p)
        {
            int drop = 0;

            // Increment drop distance while the grid cells below are empty.
            while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop;
        }

        // Calculates the minimum drop distance for the entire block.
        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach (Position p in CurrentBlock.TilePositions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }

            return drop;
        }

        // Drops the block instantly to the lowest possible position.
        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock(); // Place the block after dropping it.
        }
    }
}
