using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MUGCUP
{
    public class ColliderControl : MonoBehaviour, IInitializable
    {
        public bool IsInit { get; private set; }

        [field: SerializeField] public Transform TargetTransform { get; private set; }
        
        [field: SerializeField] public Collider2D  Collider2D  { get; private set; }
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

        private IDisposable updateRotation;
        
        public void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
        }

        public void StartUpdatePhysicRotation()
        {
            StopUpdatePhysic();

            Rigidbody2D.isKinematic = false;
            
            updateRotation = Observable.EveryUpdate().Subscribe(_ =>
            {
                var _velocity = Rigidbody2D.velocity;
                var _angle    = Mathf.Atan2(_velocity.y, _velocity.x) * Mathf.Rad2Deg;

                if (TargetTransform)
                    TargetTransform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
                
            }).AddTo(this);
        }

        public void StopUpdatePhysic()
        {
            if (updateRotation != null)
            {
                updateRotation.Dispose();
                updateRotation = null;
            }
        }
    }
}
