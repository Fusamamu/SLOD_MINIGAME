using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MUGCUP
{
    public class GridSlotAnimationControl : AnimationControl
    {
        [SerializeField] private MMF_Player OnSlotMatchAnimation;
        
        public override void Init()
        {
            base.Init();

            if (OnSlotMatchAnimation)
            {
                OnSlotMatchAnimation.Initialization();
                AnimationTable.Add(AnimationName.ON_SLOT_MATCH, OnSlotMatchAnimation);
            }
        }
    }
}
