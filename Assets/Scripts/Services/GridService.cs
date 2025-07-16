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
            _grid.DestroyBlock(x, y, this, (result) =>
            {
                OnDestroyBlock?.Invoke(result);
            });
            _grid.BlockGrid[y, x].ApplyEffect(this);

            _grid.ApplyGridGravity((origin, destination) =>
            {
                OnMoveBlock?.Invoke(origin, destination);
            });

            _grid.GenerateNewBlocks((index, block) =>
            {
                OnCreateBlock?.Invoke(index, block);
            });
        }

        public IReadOnlyList<Vector2Int> GetConnectedBlocks(Vector2Int index)
        {
            return _grid.GetConnectedBlocks(index.x, index.y);
        }

        public IReadOnlyList<Vector2Int> GetSameColorBlocks(Vector2Int index)
        {
            return _grid.GetSameColorBlocks(index.x, index.y);
        }

        public void DestroyBlocks(IReadOnlyList<Vector2Int> destroyBlockList)
        {
            foreach (var index in destroyBlockList)
            {
                _grid.DestroyBlock(index.x, index.y, this, (result) =>
                {
                    OnDestroyBlock?.Invoke(result);
                });
            }
        }
    }
}
