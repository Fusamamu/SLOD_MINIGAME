using System.Collections;
using UnityEngine;

namespace MUGCUP
{
    public class LevelManager : Service
    {
        [field: SerializeField] public int CurrentLevel { get; private set; } = 1;

        private int timer;

        [SerializeField] private TopHeaderUI TopHeaderUI;

        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            StartLevel();
            
            TopHeaderUI.ScoreText.SetText("0000");
        }

        private void StartLevel()
        {
            timer = 99;
            StartCountDown();
        }

        private void StartCountDown()
        {
            StartCoroutine(CountDownCoroutine());
        }

        private IEnumerator CountDownCoroutine()
        {
            while (timer > 0)
            {
                timer--;
                yield return new WaitForSeconds(1);
                TopHeaderUI.TimerText.SetText(timer.ToString("00"));
            }
        }
    }
}
