using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace MUGCUP
{
    public class EnemyAnimationControl : AnimationControl
    {
        [SerializeField] private MMF_Player OnWalkingAnimation;
        [SerializeField] private MMF_Player OnHitAnimation;

        public Transform TargetBodyTransform;
        private Vector3 originBodyPos;
        
        public override void Init()
        {
            base.Init();
            
            if (OnWalkingAnimation)
            {
                OnWalkingAnimation.Initialization();
                AnimationTable.Add(AnimationName.ON_WALKING, OnWalkingAnimation);
            }

            if (OnHitAnimation)
            {
                OnHitAnimation.Initialization();
                AnimationTable.Add(AnimationName.ON_HIT, OnHitAnimation);
            }

            OnHitAnimation.Events.OnComplete.AddListener(() =>
            {
                TargetBodyTransform.localPosition = originBodyPos;
                PlayAnimation(AnimationName.ON_WALKING);
            });

            originBodyPos = TargetBodyTransform.localPosition;
        }
    }
}
