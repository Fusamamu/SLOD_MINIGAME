using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class SlotPool : PoolSystem<Slot>
    {
        [SerializeField] private Transform MaskTransform;
        
        public override void Initialized()
        {
            base.Initialized();
        }
        
        protected override Slot CreateObject()
        {
            var _slot = Instantiate(Prefab, MaskTransform);

            _slot.name = $"_slot_";
            _slot.SetPool(Pool);
            _slot.Init();
            
            return _slot;
        }

        protected override void OnGetObject(Slot _slot)
        {
            _slot.gameObject.SetActive(true);
            _slot.IsInPool = false;
            
            ActivePoolObjects.Add(_slot);
        }

        protected override void OnRelease(Slot _slot)
        {
            _slot.IsInPool = true;
            _slot.gameObject.SetActive(false);
        }

        protected override void OnObjectDestroyed(Slot _slot)
        {
            Destroy(_slot.gameObject);
        }
        
        public override void ClearPool()
        {
            if (ActivePoolObjects.Count > 0)
            {
                ActivePoolObjects.ForEach(_placement => _placement.ReturnToPool());
                ActivePoolObjects.Clear();
            }
        }
    }
}
