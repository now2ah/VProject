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

    public class ColorBombEffect : IBlockEffect
    {
        public void Execute(Vector2Int index, GridService gridService)
        {
            IReadOnlyList<Vector2Int> sameColorBlockList = gridService.GetSameColorBlocks(index);

            gridService.DestroyBlocks(sameColorBlockList);

            AudioManager.Instance.PlaySfx(AudioManager.ESfx.COLORBOMB);
        }
    }
}
