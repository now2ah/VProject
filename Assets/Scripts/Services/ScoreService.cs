using VProject.Domains;
using System;
using System.Collections.Generic;
using VProject.Utils;

namespace VProject.Services
{
    public class ScoreService
    {
        private const string HIGHEST_SCORE_FILE_NAME = "highestScore.json";
        private const int DEFAULT_BLOCKSCORE = 10;

        private GridService _gridService;

        private Score _currentScore;
        private List<Score> _topScoreList;

        public int HighestScore()
        {
            if (_topScoreList == null || _topScoreList.Count == 0)
                throw new Exception("Invalid Top Score List");

            return _topScoreList[0].ScoreValue;
        }
        public IReadOnlyList<Score> TopScoreList => _topScoreList;

        public event Action<int> OnCurrentScoreValueChanged;
        public event Action<int> OnHighestScoreValueChanged;

        public ScoreService(GridService gridService)
        {
            _gridService = gridService;
            _currentScore = new Score(0);
            _topScoreList = new List<Score>();
            for (int i = 0; i < 5; ++i)
            {
                _topScoreList.Add(new Score(0));
            }

            LoadScoreData();

            _gridService.OnDestroyBlock += GridService_OnDestroyBlock;
        }

        public void SaveScoreData()
        {
            int[] scoreArray = new int[_topScoreList.Count];
            for (int i = 0; i < _topScoreList.Count; ++i)
            {
                scoreArray[i] = _topScoreList[i].ScoreValue;
            }
            HighestScoreDTO dto = new HighestScoreDTO(scoreArray);
            string jsonData = JsonSerializer.Serialize(dto);
            FileManager.SaveFile(jsonData, HIGHEST_SCORE_FILE_NAME);
        }

        private void LoadScoreData()
        {
            string json = FileManager.LoadFile(HIGHEST_SCORE_FILE_NAME);

            if (null == json)
                return;

            HighestScoreDTO dto = JsonSerializer.Deserialize<HighestScoreDTO>(json) as HighestScoreDTO;
            for (int i = 0; i < _topScoreList.Count; ++i)
            {
                _topScoreList[i].ScoreValue = dto.highestScore[i];
            }

            OnHighestScoreValueChanged?.Invoke(_topScoreList[0].ScoreValue);
        }

        private void GridService_OnDestroyBlock(BreakResult result)
        {
            _currentScore.GainScore(result.rewardScore);
            OnCurrentScoreValueChanged?.Invoke(_currentScore.ScoreValue);

            for (int i = 0; i < _topScoreList.Count; ++i)
            {
                if (_topScoreList[i].ScoreValue <= _currentScore.ScoreValue)
                {
                    _topScoreList[i].ScoreValue = _currentScore.ScoreValue;

                    if (i == 0)
                    {
                        OnHighestScoreValueChanged?.Invoke(_topScoreList[0].ScoreValue);
                    }

                    break;
                }
            }
        }
    }

    [Serializable]
    public class HighestScoreDTO
    {
        public int[] highestScore;

        public HighestScoreDTO(int[] highestScore)
        {
            this.highestScore = highestScore;
        }
    }
}
