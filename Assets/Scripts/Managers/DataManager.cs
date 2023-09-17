using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MUGCUP
{
    public class DataManager : Service
    {
        [SerializeField] private TextMeshProUGUI ScoreText;

        public int HighScore    { get; private set; }
        public int CurrentScore { get; private set; }

        public bool ResetSaveHighScore;
        
        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;

            if (ResetSaveHighScore)
                PlayerPrefs.SetInt("HighScore", 0);
            else
                LoadHighScore();
        }

        public void IncreaseScore(int _value)
        {
            CurrentScore += _value;
            ScoreText.SetText(CurrentScore.ToString("0000"));
        }

        public void DecreaseScore(int _value)
        {
            CurrentScore -= _value;
            ScoreText.SetText(CurrentScore.ToString("0000"));
        }

        public void SaveHighScore()
        {
            if (CurrentScore > HighScore)
                PlayerPrefs.SetInt("HighScore", CurrentScore);
        }

        public void LoadHighScore()
        {
            HighScore = PlayerPrefs.GetInt("HighScore");
        }
    }
}
