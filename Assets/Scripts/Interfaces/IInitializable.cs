using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public interface IInitializable
    {
        public bool IsInit { get; }
        
        public void Init();
    }
}
