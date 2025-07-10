using UnityEngine;
using System.Collections.Generic;
using Grid = VProject.Domains.Grid;

namespace VProject.Services
{
    public class GridService
    {
        private const int GRID_SIZE = 5;

        private Grid _grid;

        public GridService()
        {
            _grid = new Grid(GRID_SIZE);
        }

        public void ProcessInput(int x, int y)
        {
            List<Vector2Int> connectedBlockList = _grid.GetConnectedBlocks(x, y);
            connectedBlockList.Add(new Vector2Int(x, y));
            
            foreach (var index in connectedBlockList)
            {
                _grid.DestroyBlock(x, y);
            }

            _grid.ApplyGridGravity();

            _grid.FillEmptyBlocks();
        }
    }
}
