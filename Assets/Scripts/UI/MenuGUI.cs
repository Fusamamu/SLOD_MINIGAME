using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MUGCUP
{
    public class MenuGUI : GUI
    {
        public ButtonUI CloseButton;
        
        public ButtonUI BGMButton;
        public TextMeshProUGUI BGMText;
        
        public ButtonUI SFXButton;
        public TextMeshProUGUI SFXText;
        
        public ButtonUI ResumeButton;
        public ButtonUI RestartButton;
        public ButtonUI QuitButton;

        public TextMeshProUGUI HighScoreText;

        private DataManager  dataManager;
        private AudioManager audioManager;
       
        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;

            dataManager  = ServiceLocator.Instance.Get<DataManager>();
            audioManager = ServiceLocator.Instance.Get<AudioManager>();
            
            CloseButton  .OnClickAnimation.Events.OnComplete.AddListener(Close);
            ResumeButton .OnClickAnimation.Events.OnComplete.AddListener(Close);
            
            BGMButton.OnClickAnimation.Events.OnComplete.AddListener(() =>
            {
                audioManager.ToggleBGM();
                var _isOn = audioManager.BGMOn ? "ON" : "OFF";
                BGMText.SetText($"BGM {_isOn}");
            });
            
            SFXButton.OnClickAnimation.Events.OnComplete.AddListener(() =>
            {
                audioManager.ToggleSFX();
                var _isOn = audioManager.SFXOn ? "ON" : "OFF";
                SFXText.SetText($"SFX {_isOn}");
            });
            
            RestartButton.OnClickAnimation.Events.OnComplete.AddListener(() =>
            {
                GlobalSceneManager.Instance.LoadScene(SceneName.GameplayScene);
            });
            
            QuitButton.OnClickAnimation.Events.OnComplete.AddListener(() =>
            {
                GlobalSceneManager.Instance.LoadScene(SceneName.StartMenuScene);
            });
        }
        
        public override void Open()
        {
            base.Open();
            dataManager.LoadHighScore();
            HighScoreText.SetText(dataManager.HighScore.ToString("0000"));
        }
    }
}
