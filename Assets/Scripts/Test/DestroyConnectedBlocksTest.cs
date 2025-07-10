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
        _gridService.ProcessInput(2, 2);
    }
}
