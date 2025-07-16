using System;
using UnityEngine;
using VProject.Domains;
using VProject.Views;
using Grid = UnityEngine.Grid;

namespace VProject.Utils
{
    public class BlockViewFactory : MonoBehaviour
    {
        [SerializeField] private Transform _normalBlockViewPrefab;
        [SerializeField] private Transform _colorBombBlockViewPrefab;

        public Transform GenerateBlockView(Vector3 spawnPosition, IBreakableEntity block, Grid grid)
        {
            switch (block.GetBlockType())
            {
                case EBlockType.Normal:
                    Transform newNormalBlockView = Instantiate(_normalBlockViewPrefab, spawnPosition, Quaternion.identity);
                    if (newNormalBlockView.TryGetComponent<NormalBlockView>(out NormalBlockView normalBlockView))
                    {
                        normalBlockView.Bind(block as NormalBlock, grid);
                    }
                    return newNormalBlockView;

                case EBlockType.ColorBomb:
                    Transform newColorBombBlockView = Instantiate(_colorBombBlockViewPrefab, spawnPosition, Quaternion.identity);
                    if (newColorBombBlockView.TryGetComponent<ColorBombBlockView>(out ColorBombBlockView colorBombBlockView))
                    {
                        colorBombBlockView.Bind(block as ColorBombBlock, grid);
                    }
                    return newColorBombBlockView;

                default:
                    throw new Exception("Invalid Block Type");
            }
        }
    }
}
