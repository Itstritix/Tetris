using System;
using System.Collections;
using System.Collections.Generic;

namespace Tetris.Blocks
{
    public abstract class Block
    {
        public abstract int Type { get; }
        protected abstract Position StartingPosition { get; }
        private Position CurrentPosition;
        private int rotationState;
        protected abstract Position[][] Tiles { get; }

        public Block()
        {
            CurrentPosition = new Position(StartingPosition.Row, StartingPosition.Column);
        }

        public void Move(int row, int column)
        {
            CurrentPosition.Column += column;
            CurrentPosition.Row += row;
        }

        public void DefaultState()
        {
            rotationState = 0;
            CurrentPosition.Row = StartingPosition.Row;
            CurrentPosition.Column = StartingPosition.Column;
        }

        public void BlockRotation()
        {
            rotationState = rotationState + 1 % Tiles.Length;
        }

        public IEnumerable<Position> TilesPosition()
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row + CurrentPosition.Row, p.Column + CurrentPosition.Column);
            }
        }
    }
}
