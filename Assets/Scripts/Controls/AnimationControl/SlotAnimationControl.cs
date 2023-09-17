using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MUGCUP
{
    public class SlotAnimationControl : AnimationControl
    {
        [SerializeField] private MMF_Player OnInteractedAnimation;
        [SerializeField] private MMF_Player OnDestroyedAnimation;
    }
}
