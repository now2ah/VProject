using UnityEngine;

namespace VProject.Domains
{
    public class Score
    {
        public int ScoreValue { get; set; }

        public Score(int scoreValue)
        {
            ScoreValue = scoreValue;
        }

        public void GainScore(int gainValue)
        {
            ScoreValue += gainValue;
        }
    }
}

