using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class UIManager : Service
    {
        private readonly Dictionary<Type, GUI> uiTable = new Dictionary<Type, GUI>();

        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;

            var _guis = FindObjectsOfType<GUI>();

            foreach (var _gui in _guis)
            {
                _gui.Init();
                Add(_gui);
            }
        }

        private void Add<T>(T _gui) where T : GUI
        {
            if (!uiTable.ContainsKey(_gui.GetType()))
                uiTable.Add(_gui.GetType(), _gui);
        }
        
        public T Get<T>() where T : GUI
        {
            if (uiTable.TryGetValue(typeof(T), out var _requestedGUI))
                return _requestedGUI as T;

            return null;
        }
    }
}
