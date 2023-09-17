using System;
using System.Collections;
using System.Collections.Generic;
using MPUIKIT;
using TMPro;
using UnityEngine;

namespace MUGCUP
{
    public class ItemSlot : MonoBehaviour, IInitializable
    {
        public bool IsInit { get; private set; }
        
        [field: SerializeField] public ItemType TargetItemType { get; private set; }
        [field: SerializeField] public MPImage  SpriteImage    { get; private set; }

        [field: SerializeField] public int MaxItemCount { get; private set; } = 9;

        [SerializeField] private int CurrentItemCount;
        [SerializeField] private TextMeshProUGUI ItemCount;
        
        [field: SerializeField] public ButtonUI UseItemButton { get; private set; }

        public event Action<ItemSlot, ItemType> OnItemUsed = delegate { };
        
        public void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            //Debug
            SetItemCountMax();
        }

        public void AddItem()
        {
            if(CurrentItemCount + 1 > MaxItemCount)
                return;
            
            CurrentItemCount++;
            ItemCount.SetText($"{CurrentItemCount}");
        }

        public void UseItem()
        {
            //Debug
            if (true)
            {
                OnItemUsed?.Invoke(this, TargetItemType);
                return;
            }
            
            if(CurrentItemCount - 1 < 0)
                return;
            
            CurrentItemCount--;
            ItemCount.SetText($"{CurrentItemCount}");
            
            OnItemUsed?.Invoke(this, TargetItemType);
        }

        public void SetItemCount(int _count)
        {
            if(_count < 0 || _count > MaxItemCount)
                return;

            CurrentItemCount = _count;
            ItemCount.SetText($"{CurrentItemCount}");
        }

        public void SetItemCountMax()
        {
            CurrentItemCount = MaxItemCount;
            ItemCount.SetText($"{CurrentItemCount}");
        }
    }
}
