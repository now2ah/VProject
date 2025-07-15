using System.Collections;
using System.Drawing;
using UnityEngine;
using VProject.Domains;
using static VProject.Domains.NormalBlock;
using Grid = UnityEngine.Grid;

namespace VProject.Views
{
    public class NormalBlockView : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        [SerializeField] private IBreakableEntity _blockData;
        private Grid _grid;

        public IBreakableEntity BlockData => _blockData;

        private void OnDisable()
        {
            _blockData.OnIndexChanged -= Block_OnValueChanged;
        }

        public void Bind(NormalBlock block, Grid grid)
        {
            _blockData = block;
            _grid = grid;
            _renderer.material.color = block.BlockColor;
            block.OnIndexChanged += Block_OnValueChanged;
        }

        private void Block_OnValueChanged(Vector2Int index)
        {
            //transform.position = _grid.GetCellCenterWorld(new Vector3Int(index.x, index.y));
            StartCoroutine(MoveCoroutine(_grid.GetCellCenterWorld(new Vector3Int(index.x, index.y))));
        }

        IEnumerator MoveCoroutine(Vector3 destinationPosition)
        {
            Vector3 originPosition = transform.position;
            float duration = 0.2f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                transform.position = Vector3.Lerp(originPosition, destinationPosition, t);
                yield return null;
            }
            transform.position = destinationPosition;
        }
    }
}
