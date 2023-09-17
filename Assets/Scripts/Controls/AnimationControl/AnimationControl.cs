using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MUGCUP
{
    public static class AnimationName
    {
        public static string ON_INTERACT   = "OnInteract";
        public static string ON_DESTROYED  = "OnDestroyed";
        public static string ON_IDLE       = "OnIdle";
        public static string ON_WALKING    = "OnWalking";
        public static string ON_HIT        = "OnHit";
        public static string ON_SLOT_MATCH = "OnSlotMatch";
    }

    public class AnimationControl : MonoBehaviour, IInitializable
    {
        public bool IsInit { get; private set; }

        public bool IsAnimationPlaying {
            get
            {
                if (ActiveAnimation != null && ActiveAnimation.IsPlaying)
                    return true;

                return false;
            }
        }
        
        [SerializeField] protected MMF_Player OnInteractedAnimation;
        [SerializeField] protected MMF_Player OnDestroyedAnimation;

        protected readonly Dictionary<string, MMF_Player> AnimationTable = new Dictionary<string, MMF_Player>();

        protected MMF_Player ActiveAnimation;
        
        public virtual void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            if(OnInteractedAnimation)
                AnimationTable.Add(AnimationName.ON_INTERACT  , OnInteractedAnimation);
            
            if(OnDestroyedAnimation)
                AnimationTable.Add(AnimationName.ON_DESTROYED , OnDestroyedAnimation );
        }
        
        public void PlayAnimation(string _animationName)
        {
            StopActiveAnimation();
            
            if (AnimationTable.TryGetValue(_animationName, out var _animation))
            {
                ActiveAnimation = _animation;
                
                if(ActiveAnimation.IsPlaying)
                    ActiveAnimation.StopFeedbacks();
                
                ActiveAnimation.PlayFeedbacks();
            }
        }
        
        public void StopActiveAnimation()
        {
            if (ActiveAnimation != null && ActiveAnimation.IsPlaying)
            {
                ActiveAnimation.StopFeedbacks();
                ActiveAnimation = null;
            }
        }
    }
}
