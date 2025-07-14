using TMPro;
using UnityEngine;
using VProject.Managers;

namespace VProject.Controllers
{
    public class TimerController : MonoBehaviour
    {
        [SerializeField] private PuzzleGameManager _puzzleGameManager;

        [SerializeField] private TextMeshProUGUI _startTimerText;
        [SerializeField] private TextMeshProUGUI _playTimerText;

        private int _startCount = 4;

        private void Start()
        {
            _puzzleGameManager.Timer.OnTick += Timer_OnTick;
            _puzzleGameManager.Timer.OnTimerEnded += Timer_OnTimerEnded;
        }

        private void OnEnable()
        {
            _puzzleGameManager.OnPuzzleGameStarted += PuzzleGameManager_OnPuzzleGameStarted;
        }

        private void OnDisable()
        {
            _puzzleGameManager.OnPuzzleGameStarted -= PuzzleGameManager_OnPuzzleGameStarted;
            _puzzleGameManager.Timer.OnTick -= Timer_OnTick;
            _puzzleGameManager.Timer.OnTimerEnded -= Timer_OnTimerEnded;
        }

        private void PuzzleGameManager_OnPuzzleGameStarted()
        {
            _startTimerText.gameObject.SetActive(false);
        }

        private void Timer_OnTick(int time)
        {
            if (_puzzleGameManager.IsGameStarted == false)
            {
                if (_startTimerText.gameObject.activeSelf == false)
                    _startTimerText.gameObject.SetActive(true);

                int outputTime = _startCount - time;
                _startTimerText.text = outputTime.ToString();
            }
            else
            {
                int outputTime = _puzzleGameManager.Timer.LeftTimeTick;
                _playTimerText.text = outputTime.ToString();
            }
        }

        private void Timer_OnTimerEnded()
        {
            if (_puzzleGameManager.IsGameStarted)
            {
                _playTimerText.text = "END";
            }
        }
    }
}