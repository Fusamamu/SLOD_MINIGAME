using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

            GlobalInputManager.Instance.GameController.ItemSelection.UseSlot_1st.performed += On1stClickedHandler;
            GlobalInputManager.Instance.GameController.ItemSelection.UseSlot_2nd.performed += On2ndClickedHandler;
            GlobalInputManager.Instance.GameController.ItemSelection.UseSlot_3rd.performed += On3rdClickedHandler;
            GlobalInputManager.Instance.GameController.ItemSelection.UseSlot_4st.performed += On4stClickedHandler;
            GlobalInputManager.Instance.GameController.ItemSelection.UseSlot_5st.performed += On5stClickedHandler;
        }

        private void On1stClickedHandler(InputAction.CallbackContext _callbackContext)
        {
            if (ItemSlotTable.TryGetValue(ItemType.LARGE_BULLET, out var _itemSlot))
            {
                _itemSlot.UseItemButton.PlayOnClickAnimation();
                _itemSlot.UseItem();
            }
        }
        
        private void On2ndClickedHandler(InputAction.CallbackContext _callbackContext)
        {
            if (ItemSlotTable.TryGetValue(ItemType.SMALL_BULLET, out var _itemSlot))
            {
                _itemSlot.UseItemButton.PlayOnClickAnimation();
                _itemSlot.UseItem();
            }
        }
        
        private void On3rdClickedHandler(InputAction.CallbackContext _callbackContext)
        {
            if (ItemSlotTable.TryGetValue(ItemType.TRAP, out var _itemSlot))
            {
                _itemSlot.UseItemButton.PlayOnClickAnimation();
                _itemSlot.UseItem();
            }
        }
        
        private void On4stClickedHandler(InputAction.CallbackContext _callbackContext)
        {
            if (ItemSlotTable.TryGetValue(ItemType.LARGE_AIR_SUPPORT, out var _itemSlot))
            {
                _itemSlot.UseItemButton.PlayOnClickAnimation();
                _itemSlot.UseItem();
            }
        }
        
        private void On5stClickedHandler(InputAction.CallbackContext _callbackContext)
        {
            if (ItemSlotTable.TryGetValue(ItemType.SMALL_AIR_SUPPORT, out var _itemSlot))
            {
                _itemSlot.UseItemButton.PlayOnClickAnimation();
                _itemSlot.UseItem();
            }
        }

        private void OnSlotMatchedHandler(GridSlotData _data, ItemType _type)
        {
            if (ItemSlotTable.TryGetValue(_type, out var _itemSlot))
                _itemSlot.AddItem();
        }

        private void OnDestroy()
        {
            GridSlotData.OnSlotMatched -= OnSlotMatchedHandler;
            
            GlobalInputManager.Instance.GameController.ItemSelection.UseSlot_1st.performed -= On1stClickedHandler;
            GlobalInputManager.Instance.GameController.ItemSelection.UseSlot_2nd.performed -= On2ndClickedHandler;
            GlobalInputManager.Instance.GameController.ItemSelection.UseSlot_3rd.performed -= On3rdClickedHandler;
            GlobalInputManager.Instance.GameController.ItemSelection.UseSlot_4st.performed -= On4stClickedHandler;
            GlobalInputManager.Instance.GameController.ItemSelection.UseSlot_5st.performed -= On5stClickedHandler;
        }
    }
}
