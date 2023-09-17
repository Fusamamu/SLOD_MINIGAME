using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

namespace MUGCUP
{
    public class Slot : MonoBehaviour, ISlot, IInitializable, IPoolAble<Slot>
    {
	    public bool IsInit { get; private set; }
	    
	    [field: SerializeField] public SlotContent SlotContent { get; private set; }

	    [field: SerializeField] public Vector2Int GridSlotPosition      { get; private set; }
	    [field: SerializeField] public Vector2    GridSlotWorldPosition { get; private set; }
	    
	    [field: SerializeField] public AnimationControl AnimationControl { get; private set; }
        
        public IObjectPool<Slot> Pool { get; private set; }
        
        public bool IsInPool { get; set; }

        private static SlotContent slotContentPrefab;
        
        public void Init()
        {
	        if(IsInit)
		        return;
	        IsInit = true;
	        
	        AnimationControl.Init();

	        if (SlotContent)
	        {
		        SlotContent.Init();
		        SlotContent.SetSlotOwner(this);
	        }

	        if (slotContentPrefab == null)
		        slotContentPrefab = Resources.Load<SlotContent>("Prefabs/Content");
        }

        public void CreateNewSlotContent()
        {
	        if (slotContentPrefab == null)
	        {
		        Debug.LogWarning("Could not find Slot Content prefab");
		        return;
	        }

	        var _newSlotContent = Instantiate(slotContentPrefab, transform);
	        //_newSlotContent.SetRandomIcon();
	        SetSlotContent(_newSlotContent);
        }

        public void SetSlotContent(SlotContent _content)
        {
	        SlotContent = _content;

	        if (SlotContent != null)
	        {
		        SlotContent.SetSlotOwner(this);
		        SlotContent.transform.SetParent(transform);
		        SlotContent.transform.localPosition = Vector3.zero;
	        }
        }

        public Slot SetSlotGridPos(Vector2Int _pos)
        {
	        GridSlotPosition = _pos;
	        return this;
        }

        public Slot SetSlotWorldPos(Vector2 _pos)
        {
	        GridSlotWorldPosition = _pos;
	        transform.position    = _pos;
	        return this;
        }

        public void SetPool(IObjectPool<Slot> _pool)
        {
	        Pool = _pool;
        }

        public void ReturnToPool()
        {
	        if(!IsInPool)
		        Pool?.Release(this);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
	        Gizmos.DrawSphere(GridSlotWorldPosition, 0.02f);
	        Handles.Label(GridSlotWorldPosition, $"SLOT {GridSlotPosition.x} : {GridSlotPosition.y}");
        }
#endif
    }
}
