using TMPro;
using UnityEngine;
using VProject.Managers;
using VProject.Services;

namespace VProject.UIs
{
    public class HighestScoreUI : MonoBehaviour
    {
        [SerializeField] private PuzzleGameManager _puzzleGameManager;

        [SerializeField] private TextMeshProUGUI _highestScoreValueText;

        private void OnEnable()
        {
            _puzzleGameManager.OnPuzzleGameInitialized += PuzzleGameManager_OnPuzzleGameInitialized;
        }

        private void OnDisable()
        {
            _puzzleGameManager.OnPuzzleGameInitialized -= PuzzleGameManager_OnPuzzleGameInitialized;
            _puzzleGameManager.ScoreService.OnHighestScoreValueChanged -= ScoreService_OnHighestScoreValueChanged;
        }
        private void PuzzleGameManager_OnPuzzleGameInitialized()
        {
            _puzzleGameManager.ScoreService.OnHighestScoreValueChanged += ScoreService_OnHighestScoreValueChanged;
            _highestScoreValueText.text = _puzzleGameManager.ScoreService.HighestScore().ToString();
        }

        private void ScoreService_OnHighestScoreValueChanged(int highestScoreValue)
        {
            _highestScoreValueText.text = highestScoreValue.ToString();
        }
    }
}


