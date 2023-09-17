using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class GridSlotControl : MonoBehaviour, IInitializable
    {
        public bool IsInit { get; private set; }
        
        [field: SerializeField] public int CurrentSelectedRow    { get; private set; }
        [field: SerializeField] public int CurrentSelectedColumn { get; private set; }

        [SerializeField] private SlotSelectionGizmos RowSelectionGizmos;
        [SerializeField] private SlotSelectionGizmos ColumnSelectionGizmos;

        [SerializeField] private GridSlotData GridSlotData;
        
        public void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            RowSelectionGizmos   .Init();
            ColumnSelectionGizmos.Init();
	        
            GridSlotData.Init();

            GlobalInputManager.Instance.GameController.SlotSelection.MoveUp.performed += _context =>
            {
                MoveRowSelectionUp();
            };
            
            GlobalInputManager.Instance.GameController.SlotSelection.MoveDown.performed += _context =>
            {
                MoveRowSelectionDown();
            };
            
            GlobalInputManager.Instance.GameController.SlotSelection.MoveRight.performed += _context =>
            {
                MoveColumnRight();
            };
            
            GlobalInputManager.Instance.GameController.SlotSelection.MoveLeft.performed += _context =>
            {
                MoveColumnLeft();
            };

            GlobalInputManager.Instance.GameController.SlotSelection.SlideLeft.performed += _ =>
            {
                SlideLeftSelectedRow();
            };

            GlobalInputManager.Instance.GameController.SlotSelection.SlideRight.performed += _ =>
            {
                SlideRightSelectedRow();
            };
            
            GlobalInputManager.Instance.GameController.SlotSelection.SlideUp.performed += _ =>
            {
                SlideUpSelectedColumn();
            };

            GlobalInputManager.Instance.GameController.SlotSelection.SlideDown.performed += _ =>
            {
                SlideDownSelectedColumn();
            };
        }

#region Move Row/Colum Selection Gizmos
        public void MoveRowSelectionUp()
        {
            var _nextRow = CurrentSelectedRow + 1;

            if (GridSlotData.IsInValidRowRange(_nextRow))
                CurrentSelectedRow = _nextRow;

            var _targetHeight = GridSlotData.GetRowHeightAt(CurrentSelectedRow);
            
            RowSelectionGizmos.MoveControl.MoveVertical(_targetHeight);
        }

        public void MoveRowSelectionDown()
        {
            var _nextRow = CurrentSelectedRow - 1;
            
            if (GridSlotData.IsInValidRowRange(_nextRow))
                CurrentSelectedRow = _nextRow;
            
            var _targetHeight = GridSlotData.GetRowHeightAt(CurrentSelectedRow);
            
            RowSelectionGizmos.MoveControl.MoveVertical(_targetHeight);
        }

        private void MoveColumnRight()
        {
            var _nextColumn = CurrentSelectedColumn + 1;

            if (GridSlotData.IsInValidColumnRange(_nextColumn))
                CurrentSelectedColumn = _nextColumn;

            var _targetWidth = GridSlotData.GetColumnWidthAt(CurrentSelectedColumn);
            
            ColumnSelectionGizmos.MoveControl.MoveHorizontal(_targetWidth);
        }

        private void MoveColumnLeft()
        {
            var _nextColumn = CurrentSelectedColumn - 1;
            
            if (GridSlotData.IsInValidColumnRange(_nextColumn))
                CurrentSelectedColumn = _nextColumn;
            
            var _targetWidth = GridSlotData.GetColumnWidthAt(CurrentSelectedColumn);
            
            ColumnSelectionGizmos.MoveControl.MoveHorizontal(_targetWidth);
        }
#endregion

        private void SlideLeftSelectedRow()
        {
            GridSlotData.SlideSlots(CurrentSelectedRow, CurrentSelectedColumn, GridSlotData.SlideDirection.LEFT);
        }

        private void SlideRightSelectedRow()
        {
            GridSlotData.SlideSlots(CurrentSelectedRow, CurrentSelectedColumn,  GridSlotData.SlideDirection.RIGHT);
        }

        private void SlideUpSelectedColumn()
        {
            GridSlotData.SlideSlots(CurrentSelectedRow, CurrentSelectedColumn,  GridSlotData.SlideDirection.UP);
        }

        private void SlideDownSelectedColumn()
        {
            GridSlotData.SlideSlots(CurrentSelectedRow, CurrentSelectedColumn,  GridSlotData.SlideDirection.DOWN);
        }

        public void SelectRow(int _row)
        {
            CurrentSelectedRow = _row;
        }

        public void SelectColumn(int _column)
        {
            CurrentSelectedColumn = _column;
        }
    }
}
