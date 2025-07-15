using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VProject.Domains;
using VProject.Managers;

namespace VProject.UIs
{
    public class ScoreBoardUI : MonoBehaviour
    {
        [SerializeField] private PuzzleGameManager _puzzleGameManager;
        [SerializeField] private GameObject _scoreboardTitleText;
        [SerializeField] private List<GameObject> _scoreTextObjList;
        private Image _backgroundImage;

        private void Awake()
        {
            _backgroundImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            _puzzleGameManager.OnShowScoreBoard += PuzzleGameManager_OnShowScoreBoard;
        }

        private void OnDisable()
        {
            _puzzleGameManager.OnShowScoreBoard -= PuzzleGameManager_OnShowScoreBoard;
            _backgroundImage.enabled = false;
            _scoreboardTitleText.SetActive(false);
            foreach (var scoreObj in _scoreTextObjList)
            {
                scoreObj.SetActive(false);
            }
        }

        private void PuzzleGameManager_OnShowScoreBoard(IReadOnlyList<Score> topScoreList)
        {
            _backgroundImage.enabled = true;
            _scoreboardTitleText.SetActive(true);
            for (int i = 0; i < topScoreList.Count; ++i)
            {
                if (_scoreTextObjList[i].TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI tmpText))
                {
                    tmpText.text = $"{i + 1}. {topScoreList[i].ScoreValue.ToString()}";
                    _scoreTextObjList[i].SetActive(true);
                }
            }
        }
    }
}
