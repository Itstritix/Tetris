using System;

namespace Tetris.Blocks
{
    // Class managing the queue of upcoming Tetris blocks.
    public class BlockQueue
    {
        // Array containing all possible block types.
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        private readonly Random random = new Random(); // Random generator for selecting blocks.

        // Property representing the next block to appear in the game.
        public Block NextBlock { get; private set; }

        // Constructor initializing the queue with the first random block.
        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }

        // Selects a random block from the array of blocks.
        private Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }

        // Retrieves the next block and updates the queue with a new one.
        public Block GetAndUpdate()
        {
            Block block = NextBlock;

            // Ensures the next block is not the same as the current block.
            do
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);

            return block;
        }
    }
}
