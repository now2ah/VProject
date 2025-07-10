using System;
using VProject.Util;

namespace VProject.Domains
{
    public class Grid
    {
        private Block[,] _blockGrid;
        private BlockFactory _blockFactory;

        public Block[,] BlockGrid => _blockGrid;

        public Grid(int gridSize)
        {
            _blockGrid = new Block[gridSize, gridSize];
            _blockFactory = new BlockFactory();
            for (int i = 0; i < _blockGrid.GetLength(0); ++i)
            {
                for (int j = 0; j < _blockGrid.GetLength(1); ++j)
                {
                    _blockGrid[i,j] = _blockFactory.CreateRandomBlock();
                }
            }
        }

        public void DestroyBlock(int x, int y)
        {
            _blockGrid[x,y].Destroy();
        }
    }
}
