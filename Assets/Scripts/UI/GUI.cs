using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class GUI : MonoBehaviour, IInitializable
    {
        public bool IsInit { get; protected set; }

        [SerializeField] protected Canvas Canvas;

        public virtual void Init()
        {
         
        }

        public virtual void Open()
        {
            if(Canvas)
                Canvas.enabled = true;
        }

        public virtual void Close()
        {
            if(Canvas)
                Canvas.enabled = false;
        }
    }
}
