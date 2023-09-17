using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MUGCUP
{
    public class PlayerManager : Service
    {
        public Tower Tower;

        public int CurrentHeath;
        public List<GameObject> Healths = new List<GameObject>();

        private DataManager dataManager;
        private UIManager   uiManager;

        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            Tower
                .SetPlayerManager(this)
                .Init();

            CurrentHeath = Healths.Count - 1;

            dataManager = ServiceLocator.Instance.Get<DataManager>();
            uiManager   = ServiceLocator.Instance.Get<UIManager>();
        }

        public void ReduceHealth()
        {
            if (CurrentHeath >= 0)
            {
                Healths[CurrentHeath].SetActive(false);
                CurrentHeath--;

                if (CurrentHeath < 0)
                {
                    CurrentHeath = 0;
                    Destroy(Tower.gameObject);
                    dataManager.SaveHighScore();
                    uiManager.Get<MainGUI>().GameOverUI.SetActive(true);
                }
            }
        }

        public void ResetHealth()
        {
            foreach(var _health in Healths)
                _health.SetActive(true);
        }
    }
}
