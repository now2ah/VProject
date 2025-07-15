using UnityEngine;
using System.Collections.Generic;
using Grid = VProject.Domains.Grid;
using VProject.Domains;
using System;

namespace VProject.Services
{
    public class GridService
    {
        private const int GRID_SIZE = 7;

        private Grid _grid;

        public Grid BlockGrid => _grid;

        public event Action<BreakResult> OnDestroyBlock;
        public event Action<Vector2Int, Vector2Int> OnMoveBlock;
        public event Action<Vector2Int, IBreakableEntity> OnCreateBlock;

        public GridService()
        {
            _grid = new Grid(GRID_SIZE);
        }

        public int GetGridSize() => GRID_SIZE;

        public IBreakableEntity GetBlock(int x, int y) => _grid.BlockGrid[y, x];

        public void ProcessInput(int x, int y)
        {
            List<Vector2Int> connectedBlockList = _grid.GetConnectedBlocks(x, y);
            connectedBlockList.Add(new Vector2Int(x, y));
            
            foreach (var index in connectedBlockList)
            {
                _grid.DestroyBlock(index.x, index.y, (result) =>
                {
                    OnDestroyBlock?.Invoke(result);
                });
            }

            _grid.ApplyGridGravity((origin, destination) =>
            {
                OnMoveBlock?.Invoke(origin, destination);
            });

            _grid.GenerateNewBlocks((index, block) =>
            {
                OnCreateBlock?.Invoke(index, block);
            });
        }
    }
}
