using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MUGCUP
{
    public class GridSlotData : MonoBehaviour, IInitializable
    {
        public bool IsInit { get; private set; }

        public int Width  => Size.x;
        public int Height => Size.y;

        public int EdgeOffset = 1;
        
        public Vector2 CenterOffset 
        {
            get
            {
                float _halfWidth  = (float)(Width  - 1) / 2;
                float _halfHeight = (float)(Height - 1) / 2;
                
                return new Vector2(_halfWidth, _halfHeight);
            }
        }

        public HashSet<float> ColumnWorldPos = new HashSet<float>();
        public HashSet<float> RowWorldPos    = new HashSet<float>();

        [field: SerializeField]
        public Vector2Int Size { get; private set; }

        [SerializeField] private Slot[] Slots;
        [SerializeField] private Slot SlotPrefab;

        [SerializeField] private SlotPool SlotPool;

        [SerializeField] private GridSlotAnimationControl AnimationControl;

        public bool IsSliding { get; private set; }

        public event Action<GridSlotData, ItemType> OnSlotMatched = delegate { };

        public enum SlideDirection
        {
            LEFT, RIGHT, UP, DOWN
        }

        private AudioManager    audioManager;
        private ParticleManager particleManager;
        
        public void Init()
        {
            if (IsInit)
                return;
            
            IsInit = true;
            
            SlotPool.Initialized();

            GenerateGridSlots();

            AnimationControl.Init();
            
            OnSlotMatched += (_data, _type) =>
            {
                //AnimationControl.PlayAnimation(AnimationName.ON_SLOT_MATCH);
            };
 
            audioManager    = ServiceLocator.Instance.Get<AudioManager>();
            particleManager = ServiceLocator.Instance.Get<ParticleManager>();
        }

        public void SetSize(Vector2Int _size)
        {
            Size = _size;
        }

        public void GenerateGridSlots()
        {
            Slots = new Slot[Size.x * Size.y];

            for (var _row = 0; _row < Size.y; _row++)
                for (var _column = 0; _column < Size.x; _column++)
                {
                    Slot _slot = SlotPool.Pool?.Get();

                    if (_slot)
                    {
                        _slot.SlotContent.SetRandomIcon();
                        
                        var _slotWorldPos = new Vector2(_column, _row) - CenterOffset;
                        
                        if(!RowWorldPos.Contains(_slotWorldPos.y))
                            RowWorldPos.Add(_slotWorldPos.y);
                        if(!ColumnWorldPos.Contains(_slotWorldPos.x))
                            ColumnWorldPos.Add(_slotWorldPos.x);
                        
                        _slot
                            .SetSlotGridPos (new Vector2Int(_column, _row))
                            .SetSlotWorldPos(_slotWorldPos);

                        Slots[_row * Width + _column] = _slot;
                    }
                }
        }

        public void ClearGridSlots()
        {
            foreach (var _slot in Slots)
            {
                if(_slot == null)
                    continue;

                if (Application.isPlaying)
                    Destroy(_slot.gameObject);
                else
                    DestroyImmediate(_slot.gameObject);
            }

            Slots = null;
        }

        public void SlideSlots(int _row, int _column,  SlideDirection _direction)
        {
            if(IsSliding)
                return;
            IsSliding = true;

            StartCoroutine(SlideSlotsCoroutine(_row, _column, _direction));
        }

        private IEnumerator SlideSlotsCoroutine(int _row, int _column, SlideDirection _direction)
        {
            List<Slot> _selectedSlots = new();

            if (_direction is SlideDirection.LEFT or SlideDirection.RIGHT)
                _selectedSlots = GetSlotsAtRow(_row).ToList();

            if (_direction is SlideDirection.UP or SlideDirection.DOWN)
                _selectedSlots = GetSlotsAtColumn(_column).ToList();

            foreach (var _slot in _selectedSlots)
            {
                switch (_direction)
                {
                    case SlideDirection.LEFT:
                        _slot.SlotContent.MoveControl.Move(Vector2.left);
                        break;
                    case SlideDirection.RIGHT:
                        _slot.SlotContent.MoveControl.Move(Vector2.right);
                        break;
                    case SlideDirection.UP:
                        _slot.SlotContent.MoveControl.Move(Vector2.up);
                        break;
                    case SlideDirection.DOWN:
                        _slot.SlotContent.MoveControl.Move(Vector2.down);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_direction), _direction, null);
                }
            }

            yield return new WaitUntil(() =>
            {
                return _selectedSlots.All(_slot => !_slot.SlotContent.MoveControl.IsMoving);
            });
            
            if(_direction is SlideDirection.LEFT or SlideDirection.DOWN)
                UpdateSlotContentsForward(_selectedSlots);
            
            if(_direction is SlideDirection.RIGHT or SlideDirection.UP)
                UpdateSlotContentsBackward(_selectedSlots);
            

            //Need to use stack check row match by sequence
            for (var _rowIndex = 1; _rowIndex < Size.y - 1; _rowIndex++)
            {
                bool _isRowMatched = true;
                
                var _rowSlots = GetSlotsAtRow(_rowIndex).ToList();

                var _checkedSlot = _rowSlots[1];
                
                for (var _j = 2; _j < _rowSlots.Count - 1; _j++)
                {
                    if (!_checkedSlot.SlotContent.Equals(_rowSlots[_j].SlotContent))
                    {
                        _isRowMatched = false;
                        break;
                    }
                }

                if (_isRowMatched)
                {
                    var _matchType = _rowSlots[1].SlotContent.ItemType;
                    
                    AnimationControl.PlayAnimation(AnimationName.ON_SLOT_MATCH);
                    audioManager.Play(SoundType.SLOT_MATCHED);
                    
                    foreach (var _slot in _rowSlots)
                        _slot.SlotContent.AnimationControl.PlayAnimation(AnimationName.ON_INTERACT);

                    foreach (var _slot in _rowSlots)
                    {
                        yield return _slot.SlotContent.SelfDestroyedCoroutine();
                        
                        audioManager.Play(SoundType.SLOT_DESTROYED);
                        
                        var _particleUnit = particleManager.Pool.Pool?.Get();
                        if (_particleUnit)
                        {
                            _particleUnit.transform.position = _slot.GridSlotWorldPosition;
                            _particleUnit
                                .SelectParticle(ParticleType.LARGE_BULLET_HIT)
                                .Play();
                        }
                    }
                    
                    OnSlotMatched?.Invoke(this, _matchType);
                    
                    foreach (var _slot in GetSlotsAboveRow(_rowIndex))
                        _slot.SlotContent.MoveControl.Move(Vector2.down);
                    
                    for (var _i = _rowIndex; _i < Height - 1; _i++)
                    {
                        var _currentRow = GetSlotsAtRow(_i)    .ToList();
                        var _aboveRow   = GetSlotsAtRow(_i + 1).ToList();

                        for (var _j = 0; _j < _currentRow.Count; _j++)
                        {
                            var _slot      = _currentRow[_j];
                            var _aboveSlot = _aboveRow  [_j];
                            
                            _slot.SetSlotContent(_aboveSlot.SlotContent);
                        }
                    }

                    var _topRow = GetSlotsAtRow(Height - 1);
                    foreach (var _slot in _topRow)
                        _slot.CreateNewSlotContent();
                    
                    break;
                }
            }
            
            for (var _columnIndex = 1; _columnIndex < Size.x - 1; _columnIndex++)
            {
                bool _isColumnMatched = true;
            
                var _columnSlots = GetSlotsAtColumn(_columnIndex).ToList();
            
                var _checkedSlot = _columnSlots[1];
                
                for (var _j = 2; _j < _columnSlots.Count - 1; _j++)
                {
                    if (!_checkedSlot.SlotContent.Equals(_columnSlots[_j].SlotContent))
                    {
                        _isColumnMatched = false;
                        break;
                    }
                }

                if (_isColumnMatched)
                {
                    var _matchType = _columnSlots[1].SlotContent.ItemType;
                    
                    AnimationControl.PlayAnimation(AnimationName.ON_SLOT_MATCH);
                    audioManager.Play(SoundType.SLOT_MATCHED);
                    
                    foreach (var _slot in _columnSlots)
                        _slot.SlotContent.AnimationControl.PlayAnimation(AnimationName.ON_INTERACT);

                    foreach (var _slot in _columnSlots)
                    {
                        yield return _slot.SlotContent.SelfDestroyedCoroutine();
                        
                        audioManager.Play(SoundType.SLOT_DESTROYED);
                        
                        var _particleUnit = particleManager.Pool.Pool?.Get();
                        if (_particleUnit)
                        {
                            _particleUnit.transform.position = _slot.GridSlotWorldPosition;
                            _particleUnit
                                .SelectParticle(ParticleType.LARGE_BULLET_HIT)
                                .Play();
                        }
                    }
                    
                    OnSlotMatched?.Invoke(this, _matchType);
                    
                    foreach (var _slot in GetSlotsOnRightColumn(_columnIndex))
                        _slot.SlotContent.MoveControl.Move(Vector2.left);
                    
                    for (var _i = _columnIndex; _i < Width - 1; _i++)
                    {
                        var _currentRow  = GetSlotsAtColumn(_i)    .ToList();
                        var _rightColumn = GetSlotsAtColumn(_i + 1).ToList();
                    
                        for (var _j = 0; _j < _currentRow.Count; _j++)
                        {
                            var _slot      = _currentRow [_j];
                            var _rightSlot = _rightColumn[_j];
                            
                            _slot.SetSlotContent(_rightSlot.SlotContent);
                        }
                    }

                    var _mostRightColumn = GetSlotsAtColumn(Width - 1);
                    foreach (var _slot in _mostRightColumn)
                        _slot.CreateNewSlotContent();
                    
                    break;
                }
            }

            IsSliding = false;
        }

        private IEnumerator NiceEffect()
        {
            for (var _i = 0; _i < Size.y; _i++)
            {
                var _slots = GetSlotsAtRow(_i);

                foreach (var _slot in _slots)
                {
                    _slot.SlotContent.AnimationControl.PlayAnimation(AnimationName.ON_INTERACT);
                    yield return null;
                }
            }
            
            for (var _i = 0; _i < Size.x; _i++)
            {
                var _slots = GetSlotsAtColumn(_i);

                foreach (var _slot in _slots)
                {
                    _slot.SlotContent.AnimationControl.PlayAnimation(AnimationName.ON_INTERACT);
                    yield return null;
                }
            }
        }

        private void UpdateSlotContentsForward(List<Slot> _slots)
        {
            var _firstSlot = _slots.First();
            var _lastSlot  = _slots.Last();
            
            var _tempContentHolder = _firstSlot.SlotContent;

            for (var _i = 0; _i < _slots.Count - 1; _i++)
            {
                var _slot     = _slots[_i];
                var _nextSlot = _slots[_i + 1];

                _slot.SetSlotContent(_nextSlot.SlotContent);
            }

            _lastSlot.SetSlotContent(_tempContentHolder);
        }
        
        private void UpdateSlotContentsBackward(List<Slot> _slots)
        {
            var _firstSlot = _slots.First();
            var _lastSlot  = _slots.Last();
         
            var _tempContentHolder = _lastSlot.SlotContent;

            for (var _i = _slots.Count - 1; _i > 0; _i--)
            {
                var _slot     = _slots[_i];
                var _nextSlot = _slots[_i - 1];
                    
                _slot.SetSlotContent(_nextSlot.SlotContent);
            }

            _firstSlot.SetSlotContent(_tempContentHolder);
        }
        
        public float GetRowHeightAt(int _rowIndex)
        {
            return RowWorldPos.ElementAt(_rowIndex);
        }

        public float GetColumnWidthAt(int _columnIndex)
        {
            return ColumnWorldPos.ElementAt(_columnIndex);
        }

        private bool TryGetSlot(Slot _slot, Vector2Int _direction, out Slot _targetSlot)
        {
            var _targetSlotGridPos = _slot.GridSlotPosition + _direction;

            var _targetRow    = _targetSlotGridPos.y;
            var _targetColumn = _targetSlotGridPos.x;

            if (TryGetSlotAt(_targetRow, _targetColumn, out _targetSlot))
                return true;
            return false;
        }

        private bool TryGetSlotAt(int _row, int _column, out Slot _slot)
        {
            if (_row    < 0 || _row    > Height || 
                _column < 0 || _column > Width)
            {
                _slot = null;
                return false;
            }
                
            _slot = Slots[_row * Width + _column];
            if (_slot)
                return true;
            return false;
        }
        
        private IEnumerable<Slot> GetSlotsOnRightColumn(int _targetColumn)
        {
            for (var _i = _targetColumn + 1; _i < Width; _i++)
            {
                foreach (var _slot in GetSlotsAtColumn(_i))
                    yield return _slot;
            }
        }

        private IEnumerable<Slot> GetSlotsAboveRow(int _targetRow)
        {
            for (var _i = _targetRow + 1; _i < Height; _i++)
            {
                foreach (var _slot in GetSlotsAtRow(_i))
                    yield return _slot;
            }
        }

        public IEnumerable<Slot> GetSlotsAtRow(int _targetRow)
        {
            for (var _column = 0; _column < Width; _column++)
            {
                var _slot = Slots[_targetRow * Width + _column];
                yield return _slot;
            }
        }
        
        public IEnumerable<Slot> GetSlotsAtColumn(int _targetColumn)
        {
            for (var _row = 0; _row < Height; _row++)
            {
                var _slot = Slots[_row * Width + _targetColumn];
                yield return _slot;
            }
        }

        public bool IsInValidRowRange(int _index)
        {
            return _index <= Height - EdgeOffset - 1 && _index >= EdgeOffset;
        }

        public bool IsInValidColumnRange(int _index)
        {
            return _index <= Width - EdgeOffset - 1 && _index >= EdgeOffset;
        }

        public void PlaySlotFeedbackAt(int _row)
        {
            foreach (var _slot in GetSlotsAtRow(_row))
                _slot.AnimationControl.PlayAnimation(AnimationName.ON_INTERACT);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            
        }
#endif
    }
}
