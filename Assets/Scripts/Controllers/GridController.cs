using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using VProject.Services;
using VProject.Utils;
using VProject.Views;

public class GridController : MonoBehaviour
{
    [SerializeField] private Transform _blockPrefab;
    private GridService _gridService;
    private Grid _grid;
    [SerializeField] private List<Transform> _blockList;

    private void Awake()
    {
        _gridService = new GridService();
        _grid = GetComponent<Grid>();
        _blockList = new List<Transform>();
    }

    private void OnEnable()
    {
        InputHandler.OnClickAction += InputHandler_OnClickAction;
        _gridService.OnDestroyBlock += GridService_OnDestroyBlock;
        _gridService.OnMoveBlock += GridService_OnMoveBlock;
        _gridService.OnCreateBlock += GridService_OnCreateBlock;
    }

    private void OnDisable()
    {
        InputHandler.OnClickAction -= InputHandler_OnClickAction;
        _gridService.OnDestroyBlock -= GridService_OnDestroyBlock;
        _gridService.OnMoveBlock -= GridService_OnMoveBlock;
        _gridService.OnCreateBlock -= GridService_OnCreateBlock;
    }

    void Start()
    {
        for (int y = 0; y < _gridService.GetGridSize(); ++y)
        {
            for (int x = 0; x < _gridService.GetGridSize(); ++x)
            {
                Vector3 spawnPosition = _grid.GetCellCenterWorld(new Vector3Int(x, y));
                Transform newBlock = Instantiate(_blockPrefab, spawnPosition, Quaternion.identity);
                if (newBlock.TryGetComponent<BlockView>(out BlockView blockView))
                {
                    blockView.SetIndex(x, y);
                    blockView.SetColor(_gridService.GetBlock(x, y).type);
                }
                _blockList.Add(newBlock);
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
        StringBuilder logMessage = new StringBuilder();
        List<Transform> deleteBlockViews = new List<Transform>();

        foreach (var block in _blockList)
        {
            if (block.TryGetComponent<BlockView>(out BlockView blockView))
            {
                if (blockView.Index == index)
                {
                    logMessage.Append(index.ToString() + "/");
                    deleteBlockViews.Add(block);
                }
            }
        }
        Debug.Log(logMessage.Append(" deleted").ToString());

        foreach (var block in deleteBlockViews)
        {
            _blockList.Remove(block);
            Destroy(block.gameObject);
        }
    }

    private void GridService_OnMoveBlock(Vector2Int origin, Vector2Int destination)
    {
        foreach (var block in _blockList)
        {
            if (block.TryGetComponent<BlockView>(out BlockView blockView))
            {
                if (blockView.Index == origin)
                {
                    blockView.SetIndex(destination.x, destination.y);
                    block.transform.position = _grid.GetCellCenterWorld(new Vector3Int(destination.x, destination.y));
                }
            }
        }
    }

    private void GridService_OnCreateBlock(Vector2Int index, VProject.Domains.Block block)
    {
        Vector3 spawnPosition = _grid.GetCellCenterWorld(new Vector3Int(index.x, index.y));
        Transform newBlock = Instantiate(_blockPrefab, spawnPosition, Quaternion.identity);
        if (newBlock.TryGetComponent<BlockView>(out BlockView blockView))
        {
            blockView.SetIndex(index.x, index.y);
            blockView.SetColor(block.type);
        }
        _blockList.Add(newBlock);
    }
}
