using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MUGCUP
{
    public static class SceneName
    {
        public static string GameplayScene  = "GameplayScene";
        public static string StartMenuScene = "StartMenuScene";
    }
    
    public class GlobalSceneManager : MonoBehaviour
    {
        public static GlobalSceneManager Instance { get; private set; }

        [SerializeField] private MMF_Player CurtainTransition;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }

            if (CurtainTransition)
            {
                CurtainTransition.Initialization();
            }
        }

        public void LoadScene(string _name)
        {
            StartCoroutine(LoadSceneAsync(_name));
        }

        public IEnumerator LoadSceneAsync(string _name)
        {
            CurtainTransition.Direction = MMFeedbacks.Directions.TopToBottom;
            CurtainTransition.PlayFeedbacks();
            
            while (CurtainTransition.IsPlaying)
                yield return null;
            
            AsyncOperation _loadSceneAsync = SceneManager.LoadSceneAsync(_name);

            while (!_loadSceneAsync.isDone)
                yield return null;
            
            yield return new WaitForSeconds(0.33f);

            CurtainTransition.Direction = MMFeedbacks.Directions.BottomToTop;
            CurtainTransition.PlayFeedbacks();
        }
    }
}
