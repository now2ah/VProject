using UnityEngine;
using System.Collections.Generic;
using VProject.Domains;
using Grid = VProject.Domains.Grid;

namespace VProject.Services
{
    public class GridService
    {
        private const int GRID_SIZE = 5;

        private Grid _grid;

        private readonly int[] dx = { 0, 1, 0, -1 };
        private readonly int[] dy = { -1, 0, 1, 0 };

        public GridService()
        {
            _grid = new Grid(GRID_SIZE);
        }

        public List<Vector2Int> GetConnectedBlocks(int x, int y)
        {
            List<Vector2Int> connectedBlockList = new List<Vector2Int>();

            bool[,] isVisited = new bool[_grid.BlockGrid.GetLength(0), _grid.BlockGrid.GetLength(1)];

            Queue<Vector2Int> connectedBlockQueue = new Queue<Vector2Int>();

            connectedBlockQueue.Enqueue(new Vector2Int(x, y));
            while (connectedBlockQueue.Count > 0)
            {
                Vector2Int rootIndex = connectedBlockQueue.Dequeue();
                Block rootBlock = _grid.BlockGrid[rootIndex.x, rootIndex.y];
                isVisited[rootIndex.x, rootIndex.y] = true;

                for (int i = 0; i < 4; ++i)
                {
                    int newX = rootIndex.x + dx[i];
                    int newY = rootIndex.y + dy[i];

                    if (newX > 0 && newX < _grid.BlockGrid.GetLength(1) &&
                        newY > 0 && newY < _grid.BlockGrid.GetLength(0))
                    {
                        if (!isVisited[newX, newY] &&
                            _grid.BlockGrid[newX, newY].type == rootBlock.type)
                        {
                            isVisited[newX, newY] = true;
                            connectedBlockQueue.Enqueue(new Vector2Int(newX, newY));
                            connectedBlockList.Add(new Vector2Int(newX, newY));
                        }
                    }
                }
            }

            return connectedBlockList;
        }
    }
}
