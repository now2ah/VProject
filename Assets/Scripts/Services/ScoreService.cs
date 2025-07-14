using VProject.Domains;
using UnityEngine;
using System;

namespace VProject.Services
{
    public class ScoreService
    {
        private const int DEFAULT_BLOCKSCORE = 10;

        private GridService _gridService;

        private Score _currentScore;
        private Score _highestScore;

        public Score CurrentScore => _currentScore;

        public event Action<int> OnCurrentScoreValueChanged;

        public ScoreService(GridService gridService)
        {
            _gridService = gridService;
            _currentScore = new Score(0);
            _highestScore = new Score(0);

            _gridService.OnDestroyBlock += GridService_OnDestroyBlock;
        }

        public void SaveScoreData()
        {

        }

        private void LoadScoreData()
        {

        }

        private void GridService_OnDestroyBlock(Vector2Int index)
        {
            _currentScore.GainScore(DEFAULT_BLOCKSCORE);
            OnCurrentScoreValueChanged?.Invoke(_currentScore.ScoreValue);

            if (_currentScore.ScoreValue >= _highestScore.ScoreValue)
            {
                _highestScore.ScoreValue = _currentScore.ScoreValue;
            }
        }
    }
}
