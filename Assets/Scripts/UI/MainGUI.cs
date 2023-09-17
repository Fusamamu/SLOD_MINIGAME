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
        }
    }
}
