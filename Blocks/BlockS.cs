using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Blocks
{
    public class BlockS : Block
    {
        public override int Type => 5;
        Position[][] tilesPosition = new Position[][]
        {
            new Position[] {new (0, 1), new (0, 2), new (1, 0), new (1, 1)},
            new Position[] {new (0, 1), new (1, 1), new (1, 2), new (2, 2)},
            new Position[] {new (1, 1), new (1, 2), new (2, 0), new (2, 1)},
            new Position[] {new (0, 0), new (1, 0), new (1, 1), new (2, 1)}
        };

        protected override Position[][] Tiles => tilesPosition;
        protected override Position StartingPosition => new Position(0, 3);
    }
}
