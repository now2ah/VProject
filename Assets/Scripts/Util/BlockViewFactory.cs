using UnityEngine;
using VProject.Domains;
using VProject.Views;
using Grid = UnityEngine.Grid;

namespace VProject.Utils
{
    public class BlockViewFactory : MonoBehaviour
    {
        [SerializeField] private Transform _blockViewPrefab;

        public Transform GenerateBlockView(Vector3 spawnPosition, Block block, Grid grid)
        {
            Transform newBlockView = Instantiate(_blockViewPrefab, spawnPosition, Quaternion.identity);
            if (newBlockView.TryGetComponent<BlockView>(out BlockView blockView))
            {
                blockView.Bind(block, grid);
            }
            return newBlockView;
        }
    }
}
