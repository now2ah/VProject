using VProject.Domains;
using UnityEngine;
using System;
using VProject.Utils;

namespace VProject.Services
{
    public class ScoreService
    {
        private const string HIGHEST_SCORE_FILE_NAME = "highestScore.json";
        private const int DEFAULT_BLOCKSCORE = 10;

        private GridService _gridService;

        private Score _currentScore;
        private Score _highestScore;

        public Score CurrentScore => _currentScore;
        public Score HighestScore => _highestScore;

        public event Action<int> OnCurrentScoreValueChanged;
        public event Action<int> OnHighestScoreValueChanged;

        public ScoreService(GridService gridService)
        {
            _gridService = gridService;
            _currentScore = new Score(0);
            _highestScore = new Score(0);

            LoadScoreData();

            _gridService.OnDestroyBlock += GridService_OnDestroyBlock;
        }

        public void SaveScoreData()
        {
            HighestScoreDTO dto = new HighestScoreDTO(_highestScore.ScoreValue);
            string jsonData = JsonSerializer.Serialize(dto);
            FileManager.SaveFile(jsonData, HIGHEST_SCORE_FILE_NAME);
        }

        private void LoadScoreData()
        {
            string json = FileManager.LoadFile(HIGHEST_SCORE_FILE_NAME);

            if (null == json)
                return;

            HighestScoreDTO dto = JsonSerializer.Deserialize<HighestScoreDTO>(json) as HighestScoreDTO;
            _highestScore.ScoreValue = dto.highestScore;
            OnHighestScoreValueChanged?.Invoke(_highestScore.ScoreValue);
        }

        private void GridService_OnDestroyBlock(Vector2Int index)
        {
            _currentScore.GainScore(DEFAULT_BLOCKSCORE);
            OnCurrentScoreValueChanged?.Invoke(_currentScore.ScoreValue);

            if (_currentScore.ScoreValue >= _highestScore.ScoreValue)
            {
                _highestScore.ScoreValue = _currentScore.ScoreValue;
                OnHighestScoreValueChanged?.Invoke(_highestScore.ScoreValue);
            }
        }
    }

    [Serializable]
    public class HighestScoreDTO
    {
        public int highestScore;

        public HighestScoreDTO(int highestScore)
        {
            this.highestScore = highestScore;
        }
    }
}
