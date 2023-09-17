using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MUGCUP
{
    public class MainGUI : GUI
    {
        public ButtonUI MenuButton;
        public ButtonUI QuitButton;

        public GameObject GameOverUI;
        public ButtonUI ContinueButton;
        public ButtonUI CancelContinueButton;
        
        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            MenuButton.Button.onClick.AddListener(() =>
            {
                ServiceLocator.Instance.Get<UIManager>().Get<MenuGUI>().Open();
            });
            
            QuitButton.Button.onClick.AddListener(() =>
            {
                GlobalSceneManager.Instance.LoadScene("StartMenuScene");
            });
            
            ContinueButton.OnClickAnimation.Events.OnComplete.AddListener(() =>
            {
                GameOverUI.SetActive(false);
                GlobalSceneManager.Instance.LoadScene(SceneName.GameplayScene);
            });
            
            CancelContinueButton.OnClickAnimation.Events.OnComplete.AddListener(() =>
            {
                GameOverUI.SetActive(false);
                GlobalSceneManager.Instance.LoadScene(SceneName.StartMenuScene);
            });
        }
    }
}
