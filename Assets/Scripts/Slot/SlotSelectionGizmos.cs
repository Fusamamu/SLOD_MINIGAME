using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class SlotSelectionGizmos : MonoBehaviour, IInitializable
    {
        public bool IsInit { get; private set; }
       
        [field: SerializeField] public MoveControl MoveControl { get; private set; }
        
        public void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            MoveControl.Init();
        }
    }
}
