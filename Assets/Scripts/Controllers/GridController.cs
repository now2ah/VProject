using UnityEngine;
using VProject.Services;
using VProject.Views;

public class GridController : MonoBehaviour
{
    [SerializeField] private Transform _blockPrefab;
    private GridService _gridService;
    private Grid _grid;

    private void Awake()
    {
        _gridService = new GridService();
        _grid = GetComponent<Grid>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int y = 0; y < _gridService.GetGridSize(); ++y)
        {
            for (int x = 0; x < _gridService.GetGridSize(); ++x)
            {
                Vector3 spawnPosition = _grid.CellToWorld(new Vector3Int(x, y));
                Transform newBlock = Instantiate(_blockPrefab, spawnPosition, Quaternion.identity);
                if (newBlock.TryGetComponent<BlockView>(out BlockView blockView))
                {
                    blockView.SetColor(_gridService.GetBlock(x, y).type);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
