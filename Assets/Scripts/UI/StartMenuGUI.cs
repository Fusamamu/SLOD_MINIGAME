using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class StartMenuGUI : GUI
    {
        public ButtonUI StartButton;
        
        public void Start()
        {
            Init();
        }

        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;

            StartButton.OnClickAnimation.Events.OnComplete.AddListener(() =>
            {
                GlobalSceneManager.Instance.LoadScene(SceneName.GameplayScene);
            });
        }
    }
}
