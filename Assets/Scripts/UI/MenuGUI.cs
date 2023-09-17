using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class MenuGUI : GUI
    {
       
        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
      
        }
    }
}
