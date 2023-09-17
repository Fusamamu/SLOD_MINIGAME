using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace MUGCUP
{
    public class ButtonUI : MonoBehaviour, IInitializable
    {
        [field: SerializeField] public Button Button { get; private set; }
        
        public bool IsInit { get; private set; }

        [field: SerializeField] public MMF_Player OnClickAnimation { get; private set; }
        
        public void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
        }

        public void PlayOnClickAnimation()
        {
            if (OnClickAnimation && !OnClickAnimation.IsPlaying)
            {
                OnClickAnimation.PlayFeedbacks();
            }
        }
    }
}
