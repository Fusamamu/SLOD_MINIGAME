using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

namespace MUGCUP
{
    public class ApplicationStarter : MonoBehaviour
    {
        private void Awake()
        {
           
        }

        private void Start()
        {
            var _gridSlotControl = FindObjectOfType<GridSlotControl>();
            
            if(_gridSlotControl)
                _gridSlotControl.Init();
        }
    }
}
