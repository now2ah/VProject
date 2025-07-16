using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VProject.Services;
using VProject.Domains;
using VProject.Utils;
using VProject.Views;
using Grid = UnityEngine.Grid;
using System;

namespace VProject.Controllers
{
    [RequireComponent(typeof(BlockViewFactory))]
    public class GridController : MonoBehaviour
    {
        private GridService _gridService;
        private Grid _grid;
        private List<Transform> _blockViewList;
        private BlockViewFactory _blockViewFactory;
        private FXSpawner _fxSpawner;

        public GridService GridService => _gridService;

        public event Action<Vector3, int> OnGridViewGenerated;

        private void Awake()
        {
            _gridService = new GridService();
            _grid = GetComponent<Grid>();
            _blockViewList = new List<Transform>();
            _blockViewFactory = GetComponent<BlockViewFactory>();
            _fxSpawner = GetComponent<FXSpawner>();
            AlignCameraToGrid(_grid);
        }

        private void OnEnable()
        {
            _gridService.OnDestroyBlock += GridService_OnDestroyBlock;
            _gridService.OnCreateBlock += GridService_OnCreateBlock;
        }

        private void OnDisable()
        {
            _gridService.OnDestroyBlock -= GridService_OnDestroyBlock;
            _gridService.OnCreateBlock -= GridService_OnCreateBlock;
        }

        void Start()
        {
            for (int y = 0; y < _gridService.GetGridSize(); ++y)
            {
                for (int x = 0; x < _gridService.GetGridSize(); ++x)
                {
                    Vector3 spawnPosition = _grid.GetCellCenterWorld(new Vector3Int(x, y));
                    Transform newBlockView = _blockViewFactory.GenerateBlockView(spawnPosition, _gridService.GetBlock(x, y), _grid);
                    _blockViewList.Add(newBlockView);
                }
            }

            Vector3 gridCenterPosition = new Vector3
                (
                    (_grid.cellSize.x * _gridService.GetGridSize()) / 2,
                    (_grid.cellSize.y * _gridService.GetGridSize()) / 2,
                    0
                );
            OnGridViewGenerated?.Invoke(gridCenterPosition, _gridService.GetGridSize());
        }

        public void ProcessInput(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Block"))
                {
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
                    Vector3Int cellPosition = _grid.WorldToCell(worldPosition);

                    _fxSpawner.SpawnFX(worldPosition);

                    AudioManager.Instance.PlaySfx(AudioManager.ESfx.HIT);

                    _gridService.ProcessInput(cellPosition.x, cellPosition.y);
                }
            }
        }

        private void GridService_OnDestroyBlock(BreakResult result)
        {
            List<Transform> deleteBlockViews = new List<Transform>();

            foreach (var block in _blockViewList)
            {
                if (block.TryGetComponent<NormalBlockView>(out NormalBlockView blockView))
                {
                    if (blockView.BlockData.GetIndex() == result.index)
                    {
                        deleteBlockViews.Add(block);
                    }
                }
            }

            foreach (var block in deleteBlockViews)
            {
                _blockViewList.Remove(block);
                Destroy(block.gameObject);
            }
        }

        private void GridService_OnCreateBlock(Vector2Int index, IBreakableEntity block)
        {
            Vector3 spawnPosition = _grid.GetCellCenterWorld(new Vector3Int(index.x, index.y));
            Transform newBlockView = _blockViewFactory.GenerateBlockView(spawnPosition, block, _grid);
            _blockViewList.Add(newBlockView);
        }

        private void AlignCameraToGrid(Grid grid)
        {
            CameraManager.Instance.FocusOn(new Vector3
                (
                    (grid.cellSize.x * _gridService.GetGridSize()) / 2,
                    (grid.cellSize.y * _gridService.GetGridSize()) / 2,
                    0
                ),
                (Mathf.Max(grid.cellSize.x, grid.cellSize.y) * _gridService.GetGridSize()));
        }
    }

}