using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class ItemSlotGUI : GUI
    {
        public readonly Dictionary<ItemType, ItemSlot> ItemSlotTable = new Dictionary<ItemType, ItemSlot>();

        [SerializeField] private GridSlotData GridSlotData;

        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;

            var _itemSlots = GetComponentsInChildren<ItemSlot>();

            foreach (var _itemSlot in _itemSlots)
            {
                if(!ItemSlotTable.ContainsKey(_itemSlot.TargetItemType))
                    ItemSlotTable.Add(_itemSlot.TargetItemType, _itemSlot);

                _itemSlot.Init();
            }

            if (GridSlotData == null)
                GridSlotData = FindObjectOfType<GridSlotData>();
                
            GridSlotData.OnSlotMatched += OnSlotMatchedHandler;

            GlobalInputManager.Instance.GameController
                .ItemSelection.UseSlot_1st.performed += _ =>
                {
                    if (ItemSlotTable.TryGetValue(ItemType.LARGE_BULLET, out var _itemSlot))
                    {
                        _itemSlot.UseItemButton.PlayOnClickAnimation();
                        _itemSlot.UseItem();
                    }
                };
            
            GlobalInputManager.Instance.GameController
                .ItemSelection.UseSlot_2nd.performed += _ =>
            {
                if (ItemSlotTable.TryGetValue(ItemType.SMALL_BULLET, out var _itemSlot))
                {
                    _itemSlot.UseItemButton.PlayOnClickAnimation();
                    _itemSlot.UseItem();
                }
            };
            
            GlobalInputManager.Instance.GameController
                .ItemSelection.UseSlot_3rd.performed += _ =>
            {
                if (ItemSlotTable.TryGetValue(ItemType.TRAP, out var _itemSlot))
                {
                    _itemSlot.UseItemButton.PlayOnClickAnimation();
                    _itemSlot.UseItem();
                }
            };
            
            GlobalInputManager.Instance.GameController
                .ItemSelection.UseSlot_4st.performed += _ =>
            {
                if (ItemSlotTable.TryGetValue(ItemType.LARGE_AIR_SUPPORT, out var _itemSlot))
                {
                    _itemSlot.UseItemButton.PlayOnClickAnimation();
                    _itemSlot.UseItem();
                }
            };
            
            GlobalInputManager.Instance.GameController
                .ItemSelection.UseSlot_5st.performed += _ =>
            {
                if (ItemSlotTable.TryGetValue(ItemType.SMALL_AIR_SUPPORT, out var _itemSlot))
                {
                    _itemSlot.UseItemButton.PlayOnClickAnimation();
                    _itemSlot.UseItem();
                }
            };
            
        }

        private void OnSlotMatchedHandler(GridSlotData _data, ItemType _type)
        {
            if (ItemSlotTable.TryGetValue(_type, out var _itemSlot))
                _itemSlot.AddItem();
        }

        private void OnDestroy()
        {
            GridSlotData.OnSlotMatched -= OnSlotMatchedHandler;
        }
    }
}
