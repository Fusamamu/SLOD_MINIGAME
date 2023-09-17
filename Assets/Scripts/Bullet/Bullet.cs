using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace MUGCUP
{
    public enum BulletType
    {
        SMALL, LARGE, TRAP, SMALL_AIR_SUPPORT, LARGE_AIR_SUPPORT
    }

    [Serializable]
    public class BulletData
    {
        public BulletType Type;
        public Sprite Sprite;
        public List<float> LaunchForce = new List<float>();
    }
    
    public class Bullet : MonoBehaviour, IInitializable, IPoolAble<Bullet>
    {
        public bool IsInit { get; private set; }
        
        public BulletType Type { get; private set; }

        [SerializeField] private List<BulletData> BulletData = new List<BulletData>();
        [SerializeField] private float LaunchForce;

        [field: SerializeField] public SpriteRenderer  SpriteRenderer  { get; private set; }
        [field: SerializeField] public ColliderControl ColliderControl { get; private set; }

        public IObjectPool<Bullet> Pool { get; private set; }
        
        public bool IsInPool { get; set; }

        private ParticleManager particleManager;

        public void Init()
        {
            if(IsInit)
                return;
            IsInit = true;

            particleManager = ServiceLocator.Instance.Get<ParticleManager>();
        }
        
        public Bullet SelectBullet(BulletType _type)
        {
            foreach (var _data in BulletData)
            {
                if (_data.Type == _type)
                {
                    Type = _type;
                    SpriteRenderer.sprite = _data.Sprite;
                    
                    var _index = Random.Range(0, _data.LaunchForce.Count);
                    LaunchForce = _data.LaunchForce[_index];
                    break;
                }
            }
            
            return this;
        }

        public void Shoot()
        {
            if (Type == BulletType.SMALL ||
                Type == BulletType.LARGE ||
                Type == BulletType.TRAP)
            {
                ColliderControl.StartUpdatePhysicRotation();
                ColliderControl.Rigidbody2D.AddForce(transform.right * LaunchForce);
            }
            else
            {
                ColliderControl.StopUpdatePhysic();
            }
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
        
        public void SetPool(IObjectPool<Bullet> _pool)
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
            ColliderControl.StopUpdatePhysic();
        }
        
        public void OnTriggerEnter2D(Collider2D _other)
        {
            if (_other.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                if (Type == BulletType.TRAP)
                {
                    transform.rotation = Quaternion.identity;
                    ColliderControl.Rigidbody2D.isKinematic = true;
                    ColliderControl.Rigidbody2D.velocity    = Vector2.zero;
                    return;
                }
                
                particleManager.PlayParticle(ParticleType.HIT_FLOOR, transform.position);
                ReturnToPool();
            }
        }
    }
}
