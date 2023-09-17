using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class BulletPool : PoolSystem<Bullet>
    {
        [SerializeField] private Transform BulletCollection;
        
        public override void Initialized()
        {
            base.Initialized();
        }
        
        protected override Bullet CreateObject()
        {
            var _bullet = Instantiate(Prefab, BulletCollection);

            _bullet.name = $"_bullet_";
            _bullet.SetPool(Pool);
            _bullet.Init();
            
            return _bullet;
        }

        protected override void OnGetObject(Bullet _bullet)
        {
            _bullet.gameObject.SetActive(true);
            _bullet.IsInPool = false;
            _bullet.ColliderControl.Rigidbody2D.isKinematic = false;
            
            ActivePoolObjects.Add(_bullet);
        }

        protected override void OnRelease(Bullet _bullet)
        {
            _bullet.IsInPool = true;
            _bullet.gameObject.SetActive(false);

            ActivePoolObjects.Remove(_bullet);
        }

        protected override void OnObjectDestroyed(Bullet _bullet)
        {
            Destroy(_bullet.gameObject);
        }
        
        public override void ClearPool()
        {
            if (ActivePoolObjects.Count > 0)
            {
                ActivePoolObjects.ForEach(_bullet => _bullet.ReturnToPool());
                ActivePoolObjects.Clear();
            }
        }
    }
}
