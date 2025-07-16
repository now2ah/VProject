using System;
using System.Collections.Generic;
using UnityEngine;
using VProject.Utils;
using VProject.Services;

namespace VProject.Domains
{
    public class Grid
    {
        private IBreakableEntity[,] _blockGrid;
        private BlockFactory _blockFactory;

        public IBreakableEntity[,] BlockGrid => _blockGrid;

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
                    _blockGrid[y, x] = _blockFactory.CreateBlock(new Vector2Int(x, y), EBlockType.Normal);
                }
            }
        }

        public void DestroyBlock(int x, int y, GridService gridService, Action<BreakResult> callback = null)
        {
            BreakResult result = _blockGrid[y, x].Break();

            callback?.Invoke(result);
        }

        public List<Vector2Int> GetConnectedColorBlocks(int x, int y)
        {
            List<Vector2Int> connectedBlockList = new List<Vector2Int>();

            bool[,] isVisited = new bool[_blockGrid.GetLength(0), _blockGrid.GetLength(1)];

            Queue<Vector2Int> connectedBlockQueue = new Queue<Vector2Int>();

            connectedBlockQueue.Enqueue(new Vector2Int(x, y));
            while (connectedBlockQueue.Count > 0)
            {
                Vector2Int rootIndex = connectedBlockQueue.Dequeue();
                IBreakableEntity rootBlock = _blockGrid[rootIndex.y, rootIndex.x];

                if (rootBlock is IColorBlock == false)
                    throw new Exception("Invalid root color block");

                isVisited[rootIndex.y, rootIndex.x] = true;

                for (int i = 0; i < 4; ++i)
                {
                    int newX = rootIndex.x + dx[i];
                    int newY = rootIndex.y + dy[i];

                    if (newX >= 0 && newX < _blockGrid.GetLength(1) &&
                        newY >= 0 && newY < _blockGrid.GetLength(0))
                    {
                        if (!isVisited[newY, newX])
                        {
                            if (_blockGrid[newY, newX] is IColorBlock == false)
                                continue;

                            if (((IColorBlock)_blockGrid[newY, newX]).GetBlockColor() == ((IColorBlock)rootBlock).GetBlockColor())
                            {
                                isVisited[newY, newX] = true;
                                connectedBlockQueue.Enqueue(new Vector2Int(newX, newY));
                                connectedBlockList.Add(new Vector2Int(newX, newY));
                            }
                        }
                    }
                }
            }
            return connectedBlockList;
        }

        public List<Vector2Int> GetSameColorBlocks(int x, int y)
        {
            if (_blockGrid[y, x] is ColorBombBlock == false)
                throw new Exception($"Invalid Block Type : {_blockGrid[y, x].GetBlockType()}");

            Color blockColor = ((ColorBombBlock)_blockGrid[y, x]).GetBlockColor();

            List<Vector2Int> sameColorBlockList = new List<Vector2Int>();

            for (int i = 0; i < _blockGrid.GetLength(1); ++i)
            {
                for (int j = 0; j < _blockGrid.GetLength(0); ++j)
                {
                    if (_blockGrid[j, i] is IColorBlock)
                    {
                        if (blockColor == ((IColorBlock)_blockGrid[j, i]).GetBlockColor())
                        {
                            sameColorBlockList.Add(new Vector2Int(i, j));
                        }
                    }
                }
            }
            return sameColorBlockList;
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

        public void GenerateNewBlocks(Action<Vector2Int, IBreakableEntity> onCreateCallback)
        {
            List<Block> fallBlockList = new List<Block>();

            for (int y = 0; y < _blockGrid.GetLength(0); ++y)
            {
                for (int x = 0; x < _blockGrid.GetLength(1); ++x)
                {
                    if (_blockGrid[y, x].IsEmptyBlock())
                    {
                        Block newBlock = null;
                        Vector2Int newBlockIndex = new Vector2Int(x, y);

                        System.Random random = new System.Random();
                        int randomNumber = random.Next(0, 100);
                        if (randomNumber >= 99)
                        {
                            newBlock = _blockFactory.CreateBlock(newBlockIndex, EBlockType.ColorBomb);
                        }
                        else
                        {
                            newBlock = _blockFactory.CreateBlock(newBlockIndex, EBlockType.Normal);
                        }

                        fallBlockList.Add(newBlock);
                        _blockGrid[y, x] = newBlock;

                        Vector2Int newBlockFallIndex = new Vector2Int(newBlockIndex.x, newBlockIndex.y + 2);

                        onCreateCallback?.Invoke(newBlockFallIndex, _blockGrid[y, x]);
                    }
                }
            }

            foreach (var block in fallBlockList)
            {
                block.ChangeIndex(block.GetIndex());
            }
        }

        private void MoveBlock(Vector2Int origin, Vector2Int destination)
        {
            IBreakableEntity originBlock = _blockGrid[origin.y, origin.x];
            _blockGrid[destination.y, destination.x] = originBlock;
            originBlock.ChangeIndex(destination);
        }

        private void SetEmptyBlock(int x, int y)
        {
            _blockGrid[y, x] = _blockFactory.CreateBlock(new Vector2Int(x, y), EBlockType.None);
        }
    }
}
