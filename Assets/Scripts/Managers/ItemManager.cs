using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class ItemManager : Service
    {
        private Dictionary<string, Item[]> itemTable = new Dictionary<string, Item[]>();

        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
        }
    }
}
