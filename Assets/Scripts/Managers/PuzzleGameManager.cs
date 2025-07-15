using System;
using System.Collections.Generic;
using UnityEngine;
using VProject.Domains;
using VProject.Services;
using VProject.Controllers;
using VProject.Utils;
using UnityEngine.SceneManagement;

namespace VProject.Managers
{
    public enum EGameState
    {
        Ready,
        InGame,
        End,
    }

    public class PuzzleGameManager : MonoBehaviour
    {
        private const float DEFAULT_STARTTIME = 4f;
        private const float DEFAULT_PLAYTIME = 10f;

        [SerializeField] private GridController _gridController;
        [SerializeField] private InputHandler _inputHandler;

        private ScoreService _scoreService;

        private Timer _timer;
        private EGameState _gameState;

        public Timer Timer => _timer;
        public EGameState GameState => _gameState;

        public ScoreService ScoreService => _scoreService;

        public event Action OnPuzzleGameInitialized;
        public event Action OnPuzzleGameStarted;
        public event Action OnPuzzleGameEnded;
        public event Action<IReadOnlyList<Score>> OnShowScoreBoard;

        private void Awake()
        {
            _timer = gameObject.AddComponent<Timer>();
        }

        private void OnEnable()
        {
            InputHandler.OnClickAction += InputHandler_OnClickAction;
        }

        private void OnDisable()
        {
            InputHandler.OnClickAction -= InputHandler_OnClickAction;
        }

        private void InputHandler_OnClickAction()
        {
            if (_gameState == EGameState.End)
            {
                SceneManager.LoadSceneAsync("MainScene");
            }
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
            if (_gameState == EGameState.Ready)
            {
                StartPuzzleGame();
            }
            else
            {
                EndPuzzleGame();
                Invoke("ShowScoreBoard", 2.0f);
            }
        }

        private void StartPuzzleGame()
        {
            _inputHandler.Enabled = true;
            _gameState = EGameState.InGame;
            _timer.StartTimer(DEFAULT_PLAYTIME);
            OnPuzzleGameStarted?.Invoke();
        }

        private void EndPuzzleGame()
        {
            _inputHandler.Enabled = false;
            _gameState = EGameState.End;
            _scoreService.SaveScoreData();
            OnPuzzleGameEnded?.Invoke();
        }

        private void ShowScoreBoard()
        {
            _inputHandler.Enabled = true;
            OnShowScoreBoard?.Invoke(_scoreService.TopScoreList);
        }
    }
}
