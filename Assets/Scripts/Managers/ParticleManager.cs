using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class ParticleManager : Service
    {
        [field: SerializeField] public ParticlePool Pool { get; private set; }
        
        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            Pool.Initialized();
        }

        public void PlayParticle(ParticleType _type, Vector3 _targetPos)
        {
            var _particleUnit = ServiceLocator.Instance.Get<ParticleManager>().Pool.Pool?.Get();
            if (_particleUnit)
            {
                _particleUnit.transform.position = _targetPos;
                _particleUnit
                    .SelectParticle(_type)
                    .Play();
            }
        }
    }
}
