using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class PlayerManager : Service
    {
        public Tower Tower;
        
        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            Tower.Init();
        }
    }
}
