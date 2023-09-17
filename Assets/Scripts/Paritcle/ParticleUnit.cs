using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MUGCUP
{
    public enum ParticleType
    {
        SMALL_BULLET_HIT, 
        LARGE_BULLET_HIT,
        SHOOTING_LARGE_BULLET,
        HIT_FLOOR
    }

    [Serializable]
    public class ParticlePair
    {
        public ParticleType Type;
        public ParticleSystem ParticleSystem;
    }

    public class ParticleUnit : MonoBehaviour, IInitializable, IPoolAble<ParticleUnit>
    {
        public bool IsInit { get; private set; }

        [SerializeField] private ParticleType ParticleType;

        private ParticleSystem currentParticle;

        public List<ParticlePair> ParticleTable = new List<ParticlePair>();

        public IObjectPool<ParticleUnit> Pool { get; private set; }
        
        public bool IsInPool { get; set; }

        public void Init()
        {
            if(IsInit)
                return;
            IsInit = true;

            foreach (var _pair in ParticleTable)
                _pair.ParticleSystem.Stop();
        }

        public ParticleUnit SelectParticle(ParticleType _type)
        {
            ParticleType = _type;

            foreach (var _pair in ParticleTable)
            {
                if (_pair.Type == _type)
                {
                    currentParticle = _pair.ParticleSystem;
                    _pair.ParticleSystem.gameObject.SetActive(true);
                }
                else
                    _pair.ParticleSystem.gameObject.SetActive(false);
            }

            return this;
        }

        public void Play()
        {
            if(currentParticle)
                currentParticle.Play();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
        
        public void SetPool(IObjectPool<ParticleUnit> _pool)
        {
            Pool = _pool;
        }

        public void ReturnToPool()
        {
            if(!IsInPool)
                Pool?.Release(this);
        }

        private void OnDestroy()
        {
        }
    }
}
