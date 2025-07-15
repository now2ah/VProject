using System;
using System.Collections.Generic;
using UnityEngine;
using VProject.Domains;
using VProject.Services;
using VProject.Controllers;
using VProject.Utils;

namespace VProject.Managers
{
    public class PuzzleGameManager : MonoBehaviour
    {
        private const float DEFAULT_STARTTIME = 4f;
        private const float DEFAULT_PLAYTIME = 10f;

        [SerializeField] private GridController _gridController;
        [SerializeField] private InputHandler _inputHandler;

        private ScoreService _scoreService;

        private Timer _timer;
        private bool _isGameStarted = false;

        public Timer Timer => _timer;
        public bool IsGameStarted => _isGameStarted;

        public ScoreService ScoreService => _scoreService;

        public event Action OnPuzzleGameInitialized;
        public event Action OnPuzzleGameStarted;
        public event Action OnPuzzleGameEnded;
        public event Action<IReadOnlyList<Score>> OnShowScoreBoard;

        private void Awake()
        {
            _timer = gameObject.AddComponent<Timer>();
        }

        private void Start()
        {
            _inputHandler.Enabled = false;

            _scoreService = new ScoreService(_gridController.GridService);
            _timer.StartTimer(DEFAULT_STARTTIME);
            _timer.OnTimerEnded += Timer_OnTimerEnded;

            OnPuzzleGameInitialized?.Invoke();
        }

        private void Timer_OnTimerEnded()
        {
            if (_isGameStarted == false)
            {
                StartPuzzleGame();
            }
            else
            {
                EndPuzzleGame();
                Invoke("ShowScoreBoard", 2.0f);
            }
        }

        private void OnDisable()
        {
            _scoreService.SaveScoreData();
        }

        private void StartPuzzleGame()
        {
            _inputHandler.Enabled = true;
            _isGameStarted = true;
            _timer.StartTimer(DEFAULT_PLAYTIME);
            OnPuzzleGameStarted?.Invoke();
        }

        private void EndPuzzleGame()
        {
            _inputHandler.Enabled = false;
            _isGameStarted = false;
            OnPuzzleGameEnded?.Invoke();
        }

        private void ShowScoreBoard()
        {
            OnShowScoreBoard?.Invoke(_scoreService.TopScoreList);
        }
    }
}
