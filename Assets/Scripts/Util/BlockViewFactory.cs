using System;
using UnityEngine;
using VProject.Domains;
using VProject.Views;
using Grid = UnityEngine.Grid;

namespace VProject.Utils
{
    public class BlockViewFactory : MonoBehaviour
    {
        [SerializeField] private Transform _blockViewPrefab;

        public Transform GenerateBlockView(Vector3 spawnPosition, IBreakableEntity block, Grid grid)
        {
            switch (block.GetBlockType())
            {
                case EBlockType.Normal:
                    Transform newBlockView = Instantiate(_blockViewPrefab, spawnPosition, Quaternion.identity);
                    if (newBlockView.TryGetComponent<NormalBlockView>(out NormalBlockView blockView))
                    {
                        blockView.Bind(block as NormalBlock, grid);
                    }
                    return newBlockView;
                default:
                    throw new Exception("Invalid Block Type");
            }
        }
    }
}
