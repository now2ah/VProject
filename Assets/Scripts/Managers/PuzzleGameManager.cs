using System;
using UnityEngine;
using VProject.Services;

namespace VProject.Managers
{
    public class PuzzleGameManager : MonoBehaviour
    {
        [SerializeField] private GridController _gridController;
        private ScoreService _scoreService;

        public ScoreService ScoreService => _scoreService;

        public event Action OnPuzzleGameInitialized;

        private void Start()
        {
            _scoreService = new ScoreService(_gridController.GridService);
            OnPuzzleGameInitialized?.Invoke();
        }
    }
}
