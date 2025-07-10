using System.Collections.Generic;
using UnityEngine;
using VProject.Domains;
using VProject.Services;

public class DestroyConnectedBlocksTest : MonoBehaviour
{
    GridService _gridService;

    private void Awake()
    {
        _gridService = new GridService();
    }

    private void Start()
    {
        List<Vector2Int> connectedBlockIndex = _gridService.GetConnectedBlocks(2, 2);

        foreach (var index in connectedBlockIndex)
        {
            Debug.Log(index);
        }
    }
}
