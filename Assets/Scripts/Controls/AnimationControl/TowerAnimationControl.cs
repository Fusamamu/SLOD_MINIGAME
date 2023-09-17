using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MUGCUP
{
    public class TowerAnimationControl : AnimationControl
    {
        [SerializeField] private MMF_Player OnIdleAnimation;
        [SerializeField] private MMF_Player OnHitAnimation;
        
        public override void Init()
        {
            base.Init();
            
            if (OnIdleAnimation)
            {
                OnIdleAnimation.Initialization();
                AnimationTable.Add(AnimationName.ON_IDLE, OnIdleAnimation);
            }
            
            if(OnHitAnimation)
                AnimationTable.Add(AnimationName.ON_HIT, OnHitAnimation);
            
            OnHitAnimation.Events.OnComplete.AddListener(() =>
            {
                PlayAnimation(AnimationName.ON_IDLE);
            });
            
            PlayAnimation(AnimationName.ON_IDLE);
        }
    }
}
