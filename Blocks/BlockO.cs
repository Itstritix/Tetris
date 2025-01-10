using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Blocks
{
    public class BlockO : Block
    {
        public override int Type => 4;
        protected override Position StartingPosition => new Position(0, 4);
        Position[][] tilesPosition => new Position[][] 
        { 
            new Position[] { new(1, 0), new(1, 1), new(1, 2), new(1, 3) }
        };
        protected override Position[][] Tiles => tilesPosition;
    }
}
