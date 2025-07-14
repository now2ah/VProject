using TMPro;
using UnityEngine;
using VProject.Managers;
using VProject.Services;

namespace VProject.UIs
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private PuzzleGameManager _puzzleGameManager;

        [SerializeField] private TextMeshProUGUI _currentScoreValueText;

        private void OnEnable()
        {
            _puzzleGameManager.OnPuzzleGameInitialized += PuzzleGameManager_OnPuzzleGameInitialized;
        }

        private void OnDisable()
        {
            _puzzleGameManager.OnPuzzleGameInitialized -= PuzzleGameManager_OnPuzzleGameInitialized;
            _puzzleGameManager.ScoreService.OnCurrentScoreValueChanged -= ScoreService_OnCurrentScoreValueChanged;
        }

        private void PuzzleGameManager_OnPuzzleGameInitialized()
        {
            _puzzleGameManager.ScoreService.OnCurrentScoreValueChanged += ScoreService_OnCurrentScoreValueChanged;
        }

        private void ScoreService_OnCurrentScoreValueChanged(int currentScoreValue)
        {
            _currentScoreValueText.text = currentScoreValue.ToString();
        }
    }
}


