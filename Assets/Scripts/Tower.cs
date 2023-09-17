using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class Tower : MonoBehaviour, IInitializable
    {
        public bool IsInit { get; private set; }

        [SerializeField] private Transform BulletSpawnTarget;

        [field: SerializeField] public ColliderControl       ColliderControl       { get; private set; }
        [field: SerializeField] public TowerAnimationControl TowerAnimationControl { get; private set; }

        private PlayerManager playerManager;
        private AudioManager  audioManager;
        private BulletManager bulletManager;

        private bool keepShooting;

        public Tower SetPlayerManager(PlayerManager _playerManager)
        {
            playerManager = _playerManager;
            return this;
        }

        public void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            ColliderControl      .Init();
            TowerAnimationControl.Init();

            audioManager  = ServiceLocator.Instance.Get<AudioManager>();
            bulletManager = ServiceLocator.Instance.Get<BulletManager>();

            var _itemSlotGUI = ServiceLocator.Instance.Get<UIManager>().Get<ItemSlotGUI>();
            foreach (var _kvp in _itemSlotGUI.ItemSlotTable)
                _kvp.Value.OnItemUsed += OnShootHandler;
        }

        private void OnShootHandler(ItemSlot _itemSlot, ItemType _type)
        {
            switch (_type)
            {
                case ItemType.SMALL_BULLET:
                        bulletManager
                            .SelectBullet(BulletType.SMALL)
                            .ShootBulletFrom(BulletSpawnTarget.position);
                    break;
                case ItemType.LARGE_BULLET:
                        bulletManager
                            .SelectBullet(BulletType.LARGE)
                            .ShootBulletFrom(BulletSpawnTarget.position);
                    break;
                    
                case ItemType.TRAP:
                        bulletManager
                            .SelectBullet(BulletType.TRAP)
                            .ShootBulletFrom(BulletSpawnTarget.position);
                    break;
                    
                case ItemType.LARGE_AIR_SUPPORT:
                        bulletManager
                            .SelectBullet(BulletType.LARGE_AIR_SUPPORT)
                            .ShootAirSupport();
                    break;
                    
                case ItemType.SMALL_AIR_SUPPORT:
                        // bulletManager
                        //     .SelectBullet(BulletType.TRAP)
                        //     .ShootBulletFrom(BulletSpawnTarget.position);
                    break;
            }
        }

        private IEnumerator ShootBulletCoroutine()
        {
            while (keepShooting)
            {
                yield return new WaitForSeconds(1);

                var _newBullet = bulletManager.BulletPool.Pool?.Get();

                if (_newBullet)
                {
                    _newBullet.transform.position = BulletSpawnTarget.transform.position;
                    _newBullet.Shoot();
                }
            }
        }
        
        public void OnTriggerEnter2D(Collider2D _other)
        {
            if (_other.TryGetComponent<Enemy>(out var _enemy))
            {
                Destroy(_enemy.gameObject);
                TowerAnimationControl.PlayAnimation(AnimationName.ON_HIT);
                audioManager.Play(SoundType.TOWER_HIT);
                playerManager.ReduceHealth();
            }
        }

        private void OnDestroy()
        {
            var _itemSlotGUI = ServiceLocator.Instance.Get<UIManager>().Get<ItemSlotGUI>();
            foreach (var _kvp in _itemSlotGUI.ItemSlotTable)
                _kvp.Value.OnItemUsed -= OnShootHandler;
        }
    }
}
