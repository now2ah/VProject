using System.Collections;
using UnityEngine;
using VProject.Domains;
using Grid = UnityEngine.Grid;

namespace VProject.Views
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        [SerializeField] private Block _blockData;
        private Grid _grid;

        public Block BlockData => _blockData;

        private void OnDisable()
        {
            _blockData.OnValueChanged -= Block_OnValueChanged;
        }

        public void Bind(Block block, Grid grid)
        {
            _blockData = block;
            _grid = grid;
            SetColor(_blockData.Type);
            block.OnValueChanged += Block_OnValueChanged;
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

        private void SetColor(EBlockType type)
        {
            switch (type)
            {
                case EBlockType.Red:
                    _renderer.material.color = Color.red;
                    break;
                case EBlockType.Yellow:
                    _renderer.material.color = Color.yellow;
                    break;
                case EBlockType.Green:
                    _renderer.material.color = Color.green;
                    break;
            }
        }
    }
}
