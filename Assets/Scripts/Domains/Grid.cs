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
            for (int y = 0; y < _blockGrid.GetLength(0); ++y)
            {
                for (int x = 0; x < _blockGrid.GetLength(1); ++x)
                {
                    _blockGrid[y, x] = _blockFactory.CreateRandomBlock(new Vector2Int(x, y));
                }
            }
        }

        public void DestroyBlock(int x, int y, Action<EBlockType> callback = null)
        {
            _blockGrid[y, x].Destroy();

            callback?.Invoke(_blockGrid[y,x].Type);
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
                            _blockGrid[newY, newX].Type == rootBlock.Type)
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
                    if (_blockGrid[y, x].IsEmptyBlock())
                    {
                        for (int aboveY = y + 1; aboveY < _blockGrid.GetLength(0); ++aboveY)
                        {
                            if (_blockGrid[aboveY, x].IsEmptyBlock() == false)
                            {
                                MoveBlock(new Vector2Int(x, aboveY), new Vector2Int(x, y));
                                SetEmptyBlock(x, aboveY);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void FillEmptyBlocks(Action<Vector2Int, Block> onCreateCallback)
        {
            for (int y = 0; y < _blockGrid.GetLength(0); ++y)
            {
                for (int x = 0; x < _blockGrid.GetLength(1); ++x)
                {
                    if (_blockGrid[y, x].IsEmptyBlock())
                    {
                        Vector2Int newBlockIndex = new Vector2Int(x, y);
                        _blockGrid[y, x] = _blockFactory.CreateRandomBlock(newBlockIndex);
                        //Debug.Log($"{_blockGrid[y, x]} generated at {newBlockIndex}");
                        onCreateCallback?.Invoke(newBlockIndex, _blockGrid[y, x]);
                    }
                }
            }
        }

        private void MoveBlock(Vector2Int origin, Vector2Int destination)
        {
            Block originBlock = _blockGrid[origin.y, origin.x];
            _blockGrid[destination.y, destination.x] = originBlock;
            originBlock.ChangeIndex(destination);
        }

        private void SetEmptyBlock(int x, int y)
        {
            _blockGrid[y, x] = _blockFactory.CreateBlock(new Vector2Int(x, y), EBlockType.None);
        }
    }
}
