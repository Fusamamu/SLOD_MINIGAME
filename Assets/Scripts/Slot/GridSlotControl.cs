using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MUGCUP
{
    public class GridSlotControl : MonoBehaviour, IInitializable
    {
        public bool IsInit { get; private set; }
        
        [field: SerializeField] public int CurrentSelectedRow    { get; private set; }
        [field: SerializeField] public int CurrentSelectedColumn { get; private set; }

        [SerializeField] private SlotSelectionGizmos HorizontalCursor;
        [SerializeField] private SlotSelectionGizmos VerticalCursor;
        [SerializeField] private SlotSelectionGizmos RowSelectionGizmos;
        [SerializeField] private SlotSelectionGizmos ColumnSelectionGizmos;

        [SerializeField] private GridSlotData GridSlotData;
        
        public void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            GridSlotData.Init();
            HorizontalCursor     .Init();
            VerticalCursor       .Init();
            RowSelectionGizmos   .Init();
            ColumnSelectionGizmos.Init();
            
            var _targetHeight = GridSlotData.GetRowHeightAt  (2);
            var _targetWidth  = GridSlotData.GetColumnWidthAt(2);
            HorizontalCursor     .MoveControl.MoveHorizontalImmediate(_targetHeight);
            VerticalCursor       .MoveControl.MoveVerticalImmediate(_targetWidth);
            RowSelectionGizmos   .MoveControl.MoveVerticalImmediate  (_targetHeight);
            ColumnSelectionGizmos.MoveControl.MoveHorizontalImmediate(_targetWidth);

            GlobalInputManager.Instance.GameController.SlotSelection.MoveUp    .performed += MoveRowSelectionUp;
            GlobalInputManager.Instance.GameController.SlotSelection.MoveDown  .performed += MoveRowSelectionDown;
            GlobalInputManager.Instance.GameController.SlotSelection.MoveRight .performed += MoveColumnRight;
            GlobalInputManager.Instance.GameController.SlotSelection.MoveLeft  .performed += MoveColumnLeft;
            GlobalInputManager.Instance.GameController.SlotSelection.SlideLeft .performed += SlideLeftSelectedRow;
            GlobalInputManager.Instance.GameController.SlotSelection.SlideRight.performed += SlideRightSelectedRow;
            GlobalInputManager.Instance.GameController.SlotSelection.SlideUp   .performed += SlideUpSelectedColumn;
            GlobalInputManager.Instance.GameController.SlotSelection.SlideDown .performed += SlideDownSelectedColumn;
        }

#region Move Row/Colum Selection Gizmos
        public void MoveRowSelectionUp(InputAction.CallbackContext _callbackContext)
        {
            var _nextRow = CurrentSelectedRow + 1;

            if (GridSlotData.IsInValidRowRange(_nextRow))
                CurrentSelectedRow = _nextRow;

            var _targetHeight = GridSlotData.GetRowHeightAt(CurrentSelectedRow);
            
            RowSelectionGizmos.MoveControl.MoveVertical(_targetHeight);
            VerticalCursor    .MoveControl.MoveVertical(_targetHeight);
        }

        public void MoveRowSelectionDown(InputAction.CallbackContext _callbackContext)
        {
            var _nextRow = CurrentSelectedRow - 1;
            
            if (GridSlotData.IsInValidRowRange(_nextRow))
                CurrentSelectedRow = _nextRow;
            
            var _targetHeight = GridSlotData.GetRowHeightAt(CurrentSelectedRow);
            
            RowSelectionGizmos.MoveControl.MoveVertical(_targetHeight);
            VerticalCursor    .MoveControl.MoveVertical(_targetHeight);
        }

        private void MoveColumnRight(InputAction.CallbackContext _callbackContext)
        {
            var _nextColumn = CurrentSelectedColumn + 1;

            if (GridSlotData.IsInValidColumnRange(_nextColumn))
                CurrentSelectedColumn = _nextColumn;

            var _targetWidth = GridSlotData.GetColumnWidthAt(CurrentSelectedColumn);
            
            ColumnSelectionGizmos.MoveControl.MoveHorizontal(_targetWidth);
            HorizontalCursor     .MoveControl.MoveHorizontal(_targetWidth);
        }

        private void MoveColumnLeft(InputAction.CallbackContext _callbackContext)
        {
            var _nextColumn = CurrentSelectedColumn - 1;
            
            if (GridSlotData.IsInValidColumnRange(_nextColumn))
                CurrentSelectedColumn = _nextColumn;
            
            var _targetWidth = GridSlotData.GetColumnWidthAt(CurrentSelectedColumn);
            
            ColumnSelectionGizmos.MoveControl.MoveHorizontal(_targetWidth);
            HorizontalCursor     .MoveControl.MoveHorizontal(_targetWidth);
        }
#endregion

        private void SlideLeftSelectedRow(InputAction.CallbackContext _callbackContext)
        {
            GridSlotData.SlideSlots(CurrentSelectedRow, CurrentSelectedColumn, GridSlotData.SlideDirection.LEFT);
        }

        private void SlideRightSelectedRow(InputAction.CallbackContext _callbackContext)
        {
            GridSlotData.SlideSlots(CurrentSelectedRow, CurrentSelectedColumn,  GridSlotData.SlideDirection.RIGHT);
        }

        private void SlideUpSelectedColumn(InputAction.CallbackContext _callbackContext)
        {
            GridSlotData.SlideSlots(CurrentSelectedRow, CurrentSelectedColumn,  GridSlotData.SlideDirection.UP);
        }

        private void SlideDownSelectedColumn(InputAction.CallbackContext _callbackContext)
        {
            GridSlotData.SlideSlots(CurrentSelectedRow, CurrentSelectedColumn,  GridSlotData.SlideDirection.DOWN);
        }

        private void OnDestroy()
        {
            GlobalInputManager.Instance.GameController.SlotSelection.MoveUp    .performed -= MoveRowSelectionUp;
            GlobalInputManager.Instance.GameController.SlotSelection.MoveDown  .performed -= MoveRowSelectionDown;
            GlobalInputManager.Instance.GameController.SlotSelection.MoveRight .performed -= MoveColumnRight;
            GlobalInputManager.Instance.GameController.SlotSelection.MoveLeft  .performed -= MoveColumnLeft;
            GlobalInputManager.Instance.GameController.SlotSelection.SlideLeft .performed -= SlideLeftSelectedRow;
            GlobalInputManager.Instance.GameController.SlotSelection.SlideRight.performed -= SlideRightSelectedRow;
            GlobalInputManager.Instance.GameController.SlotSelection.SlideUp   .performed -= SlideUpSelectedColumn;
            GlobalInputManager.Instance.GameController.SlotSelection.SlideDown .performed -= SlideDownSelectedColumn;
        }
    }
}
