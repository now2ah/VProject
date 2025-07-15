using UnityEngine;
using VProject.Managers;

namespace VProject.UIs
{
    public class CoverImageUI : MonoBehaviour
    {
        [SerializeField] private GameObject _coverImageObj;
        [SerializeField] private PuzzleGameManager _puzzleGameManager;

        private void OnEnable()
        {
            _puzzleGameManager.OnPuzzleGameEnded += PuzzleGameManager_OnPuzzleGameEnded;
        }

        private void OnDisable()
        {
            _puzzleGameManager.OnPuzzleGameEnded -= PuzzleGameManager_OnPuzzleGameEnded;
            _coverImageObj.SetActive(false);
        }

        private void PuzzleGameManager_OnPuzzleGameEnded()
        {
            _coverImageObj.SetActive(true);
        }
    }
}
