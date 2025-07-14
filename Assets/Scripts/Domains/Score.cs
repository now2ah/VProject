using UnityEngine;

namespace VProject.Domains
{
    public class Score
    {
        private int _scoreValue;

        public int ScoreValue => _scoreValue;

        public void GainScore(int gainValue)
        {
            _scoreValue += gainValue;
        }
    }
}

