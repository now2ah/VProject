using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VProject.Services;
using VProject.Domains;
using VProject.Utils;
using VProject.Views;
using Grid = UnityEngine.Grid;

[RequireComponent(typeof(BlockViewFactory))]
public class GridController : MonoBehaviour
{
    private GridService _gridService;
    private Grid _grid;
    private List<Transform> _blockViewList;
    private BlockViewFactory _blockViewFactory;

    private void Awake()
    {
        _gridService = new GridService();
        _grid = GetComponent<Grid>();
        _blockViewList = new List<Transform>();
        _blockViewFactory = GetComponent<BlockViewFactory>();
    }

    private void OnEnable()
    {
        InputHandler.OnClickAction += InputHandler_OnClickAction;
        _gridService.OnDestroyBlock += GridService_OnDestroyBlock;
        //_gridService.OnMoveBlock += GridService_OnMoveBlock;
        _gridService.OnCreateBlock += GridService_OnCreateBlock;
    }

    private void OnDisable()
    {
        InputHandler.OnClickAction -= InputHandler_OnClickAction;
        _gridService.OnDestroyBlock -= GridService_OnDestroyBlock;
        //_gridService.OnMoveBlock -= GridService_OnMoveBlock;
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
    }

    private void InputHandler_OnClickAction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Block"))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
                Vector3Int cellPosition = _grid.WorldToCell(worldPosition);

                Debug.Log($"clicked cell position {cellPosition}");

                _gridService.ProcessInput(cellPosition.x, cellPosition.y);
            }
        }
    }

    private void GridService_OnDestroyBlock(Vector2Int index)
    {
        List<Transform> deleteBlockViews = new List<Transform>();

        foreach (var block in _blockViewList)
        {
            if (block.TryGetComponent<BlockView>(out BlockView blockView))
            {
                if (blockView.BlockData.Index == index)
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

    //private void GridService_OnMoveBlock(Vector2Int origin, Vector2Int destination)
    //{
    //    foreach (var block in _blockViewList)
    //    {
    //        if (block.TryGetComponent<BlockView>(out BlockView blockView))
    //        {
    //            if (blockView.BlockData.Index == origin)
    //            {
    //                blockView.SetIndex(destination.x, destination.y);
    //                block.transform.position = _grid.GetCellCenterWorld(new Vector3Int(destination.x, destination.y));
    //            }
    //        }
    //    }
    //}

    private void GridService_OnCreateBlock(Vector2Int index, Block block)
    {
        Vector3 spawnPosition = _grid.GetCellCenterWorld(new Vector3Int(index.x, index.y));
        Transform newBlockView = _blockViewFactory.GenerateBlockView(spawnPosition, block, _grid);
        _blockViewList.Add(newBlockView);
    }
}
