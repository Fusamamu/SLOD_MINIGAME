using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace MUGCUP
{
    public class Enemy : MonoBehaviour, IInitializable
    {
        public bool IsInit { get; private set; }

        [SerializeField] private int Health;
        [SerializeField] private int Score;
        
        [field: SerializeField] public MoveControl           MoveControl      { get; private set; }
        [field: SerializeField] public ColliderControl       ColliderControl  { get; private set; }
        [field: SerializeField] public EnemyAnimationControl AnimationControl { get; private set; }

        private DataManager  dataManager;
        private EnemyManager enemyManager;
        private AudioManager audioManager;
        
        public Enemy SetEnemyManager(EnemyManager _manager)
        {
            enemyManager = _manager;
            enemyManager.AddEnemy(this);
            return this;
        }

        public void Init()
        {
            if(IsInit)
                return;
            IsInit = true;

            name = $"_Enemy_";
            
            MoveControl     .Init();
            ColliderControl .Init();
            AnimationControl.Init();

            MoveControl
                .SetEaseType(Ease.Linear)
                .SetDuration(40f)
                .MoveTo(transform.position + Vector3.left * 30);
            
            AnimationControl.PlayAnimation(AnimationName.ON_WALKING);

            dataManager  = ServiceLocator.Instance.Get<DataManager>();
            audioManager = ServiceLocator.Instance.Get<AudioManager>();
        }

        public void OnTriggerEnter2D(Collider2D _other)
        {
            if (_other.TryGetComponent<Bullet>(out var _bullet))
            {
                audioManager.Play(SoundType.ENEMY_DEAD);
                
                var _particleUnit = ServiceLocator.Instance.Get<ParticleManager>().Pool.Pool?.Get();
                if (_particleUnit)
                {
                    _particleUnit.transform.position = transform.position;
                    _particleUnit
                        .SelectParticle(ParticleType.LARGE_BULLET_HIT)
                        .Play();
                }

                switch (_bullet.Type)
                {
                    case BulletType.SMALL:
                        Health--;
                        break;
                    case BulletType.LARGE:
                        Health = 0;
                        break;
                    case BulletType.TRAP:
                        Health = 0;
                        break;
                }
                
                _bullet.ReturnToPool();
                
                AnimationControl.PlayAnimation(AnimationName.ON_HIT);

                if (Health <= 0)
                {
                    dataManager.IncreaseScore(Score);
                    Destroy(gameObject);
                }
            }
        }

        public void OnDestroy()
        {
            if(enemyManager)
                enemyManager.RemoveEnemy(this);
        }
    }
}
