using System;
using System.Collections.Generic;
using UnityEngine;
using VProject.Domains;
using VProject.Services;
using VProject.Controllers;
using VProject.Utils;
using VProject.UIs;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using VProject.Views;
using System.Collections;

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
        private const float DEFAULT_PLAYTIME = 30f;

        [SerializeField] private GridController _gridController;
        [SerializeField] private InputHandler _inputHandler;

        private ScoreService _scoreService;

        private Timer _timer;
        private EGameState _gameState;

        private bool _isActive = true;
        private Coroutine _deactiveInputCoroutine;

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
            if (_isActive == false)
                return;

            if (_gameState == EGameState.InGame)
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
                _gridController.ProcessInput(ray);
            }
            else if (_gameState == EGameState.End)
            {
                SceneManager.LoadSceneAsync("MainScene");
            }

            DeactiveInputForSeconds(0.5f);
        }

        private void Start()
        {
            _inputHandler.Enabled = false;

            _scoreService = new ScoreService(_gridController.GridService);
            _timer.StartTimer(DEFAULT_STARTTIME);
            _timer.OnTimerEnded += Timer_OnTimerEnded;

            OnPuzzleGameInitialized?.Invoke();

            AudioManager.Instance.PlayBgm(AudioManager.eBgm.BGM_GAME);
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

            AudioManager.Instance.PlaySfx(AudioManager.ESfx.END);
        }

        private void ShowScoreBoard()
        {
            _inputHandler.Enabled = true;
            OnShowScoreBoard?.Invoke(_scoreService.TopScoreList);
        }

        private void DeactiveInputForSeconds(float deactiveTime)
        {
            if (_deactiveInputCoroutine != null)
                StopCoroutine(_deactiveInputCoroutine);

            _deactiveInputCoroutine = StartCoroutine(DeactiveInput(deactiveTime));
        }

        private IEnumerator DeactiveInput(float deactiveTime)
        {
            _isActive = false;
            yield return new WaitForSeconds(deactiveTime);
            _isActive = true;
        }
    }
}
