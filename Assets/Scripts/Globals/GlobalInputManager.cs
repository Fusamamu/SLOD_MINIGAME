using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class GlobalInputManager : MonoBehaviour
    {
        public static GlobalInputManager Instance { get; private set; }

        public GameController GameController;
        
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

        private void Start()
        {
            GameController = new GameController();
            GameController.Enable();
        }
    }
}
