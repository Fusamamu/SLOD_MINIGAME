using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class BulletManager : Service
    {
        [field: SerializeField] public BulletPool BulletPool { get; private set; }

        private BulletType selectedBulletType;
        
        private AudioManager audioManager;
        
        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            BulletPool.Initialized();

            audioManager = ServiceLocator.Instance.Get<AudioManager>();
        }

        public BulletManager SelectBullet(BulletType _type)
        {
            selectedBulletType = _type;
            return this;
        }

        public void ShootBulletFrom(Vector3 _targetPos)
        {
            var _newBullet = BulletPool.Pool?.Get();

            if (_newBullet)
            {
                _newBullet.SelectBullet(selectedBulletType);
                _newBullet.transform.position = _targetPos;
                _newBullet.Shoot();
                
                audioManager.Play(SoundType.SHOOT_LARGE_BULLET);
            }
        }

        public void ShootAirSupport()
        {
            for (var _i = 0; _i < 10; _i++)
            {
                var _newBullet = BulletPool.Pool?.Get();

                if (_newBullet)
                {
                    _newBullet.SelectBullet(selectedBulletType);
                    _newBullet.transform.position = new Vector3(0, 5, 0);
                    _newBullet.Shoot();
                    
                    audioManager.Play(SoundType.SHOOT_LARGE_BULLET);
                }
            }
        }
    }
}
