using System;
using System.Collections.Generic;
using UnityEngine;
using VProject.Utils;

namespace VProject.Domains
{
    public class Grid
    {
        private Block[,] _blockGrid;
        private BlockFactory _blockFactory;

        public Block[,] BlockGrid => _blockGrid;

        private readonly int[] dx = { 0, 1, 0, -1 };
        private readonly int[] dy = { -1, 0, 1, 0 };

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

        public void DestroyBlock(int x, int y, Action callback = null)
        {
            _blockGrid[y, x].Destroy();

            callback?.Invoke();
        }

        public List<Vector2Int> GetConnectedBlocks(int x, int y)
        {
            List<Vector2Int> connectedBlockList = new List<Vector2Int>();

            bool[,] isVisited = new bool[_blockGrid.GetLength(0), _blockGrid.GetLength(1)];

            Queue<Vector2Int> connectedBlockQueue = new Queue<Vector2Int>();

            connectedBlockQueue.Enqueue(new Vector2Int(x, y));
            while (connectedBlockQueue.Count > 0)
            {
                Vector2Int rootIndex = connectedBlockQueue.Dequeue();
                Block rootBlock = _blockGrid[rootIndex.y, rootIndex.x];
                isVisited[rootIndex.y, rootIndex.x] = true;

                for (int i = 0; i < 4; ++i)
                {
                    int newX = rootIndex.x + dx[i];
                    int newY = rootIndex.y + dy[i];

                    if (newX >= 0 && newX < _blockGrid.GetLength(1) &&
                        newY >= 0 && newY < _blockGrid.GetLength(0))
                    {
                        if (!isVisited[newY, newX] &&
                            _blockGrid[newY, newX].type == rootBlock.type)
                        {
                            isVisited[newY, newX] = true;
                            connectedBlockQueue.Enqueue(new Vector2Int(newX, newY));
                            connectedBlockList.Add(new Vector2Int(newX, newY));
                        }
                    }
                }
            }

            return connectedBlockList;
        }

        public void ApplyGridGravity(Action<Vector2Int, Vector2Int> onMoveCallback)
        {
            for (int x = 0; x < _blockGrid.GetLength(1); ++x)
            {
                for (int y = 0; y < _blockGrid.GetLength(0); ++y)
                {
                    if (_blockGrid[y, x].type == EBlockType.None)
                    {
                        for (int aboveY = y + 1; aboveY < _blockGrid.GetLength(0); ++aboveY)
                        {
                            if (_blockGrid[aboveY, x].type != EBlockType.None)
                            {
                                _blockGrid[y, x].type = _blockGrid[aboveY, x].type;
                                _blockGrid[aboveY, x].type = EBlockType.None;

                                onMoveCallback?.Invoke(new Vector2Int(x, aboveY), new Vector2Int(x, y));
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void FillEmptyBlocks()
        {
            for (int y = 0; y < _blockGrid.GetLength(0); ++y)
            {
                for (int x = 0; x < _blockGrid.GetLength(1); ++x)
                {
                    if (_blockGrid[y, x].type == EBlockType.None)
                    {
                        _blockGrid[y, x] = _blockFactory.CreateRandomBlock();
                    }
                }
            }
        }
    }
}
