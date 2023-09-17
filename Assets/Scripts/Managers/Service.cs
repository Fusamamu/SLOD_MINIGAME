using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class Service : MonoBehaviour, IInitializable
    {
        public bool IsInit { get; protected set; }
        
        public virtual void Init()
        {
        }
    }
}
