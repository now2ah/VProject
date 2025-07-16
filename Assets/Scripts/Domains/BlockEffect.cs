using System.Collections.Generic;
using UnityEngine;
using VProject.Services;

namespace VProject.Domains
{
    public class NormalBlockEffect : IBlockEffect
    {
        public void Execute(Vector2Int index, GridService gridService)
        {
            IReadOnlyList<Vector2Int> connectedBlockList = gridService.GetConnectedBlocks(index);

            gridService.DestroyBlocks(connectedBlockList);
        }
    }
}
