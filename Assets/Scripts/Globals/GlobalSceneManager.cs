using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MUGCUP
{
    public class GlobalSceneManager : MonoBehaviour
    {
        public static GlobalSceneManager Instance { get; private set; }
        
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
        }

        public void LoadScene(string _name)
        {
            StartCoroutine(LoadSceneAsync(_name));
        }

        public IEnumerator LoadSceneAsync(string _name)
        {
            AsyncOperation _loadSceneAsync = SceneManager.LoadSceneAsync(_name);

            while (!_loadSceneAsync.isDone)
            {
                yield return null;
            }
        }
    }
}
