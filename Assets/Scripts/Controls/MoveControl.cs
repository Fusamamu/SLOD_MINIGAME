using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace MUGCUP
{
    public class MoveControl : MonoBehaviour, IInitializable
    {
        public bool IsInit { get; private set; }

        public bool IsMoving => moveProcess != null;
        
        private Tween moveProcess;

        private Ease easeType = Ease.InOutExpo;
        
        [SerializeField] private Transform Target;
        
        [SerializeField] private float Duration = 1f;
        
        public void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
        }

        public MoveControl SetEaseType(Ease _ease)
        {
            easeType = _ease;
            return this;
        }

        public MoveControl SetDuration(float _duration)
        {
            Duration = _duration;
            return this;
        }

        public IEnumerator MoveToCoroutine(Vector3 _targetPos)
        {
            StopMove();

            moveProcess = Target
                .DOMove(_targetPos, Duration)
                .SetEase(Ease.InOutExpo);

            yield return moveProcess;
        }

        public void MoveImmediateTo(Vector3 _position)
        {
            Target.transform.position = _position;
        }
        
        public void Move(Vector3 _targetPos, TweenCallback _callback = null)
        {
            StopMove();

            moveProcess = Target
                .DOMove(Target.transform.position + _targetPos, Duration)
                .SetEase(Ease.InOutExpo)
                .OnComplete(() =>
                {
                    StopMove();
                    _callback?.Invoke();
                });
        }
        
        public void MoveTo(Vector3 _targetPos, TweenCallback _callback = null)
        {
            StopMove();

            moveProcess = Target
                .DOMove(_targetPos, Duration)
                .SetEase(easeType)
                .OnComplete(_callback);
        }

        public void MoveHorizontal(float _targetX, TweenCallback _callback = null)
        {
            StopMove();

            var _position  = Target.transform.position;
            var _targetPos = new Vector3(_targetX, _position.y, _position.z);

            moveProcess = Target
                .DOMove(_targetPos, Duration)
                .SetEase(Ease.InOutExpo)
                .OnComplete(_callback);
        }
        
        public void MoveHorizontalImmediate(float _targetX)
        {
            var _position  = Target.transform.position;
            var _targetPos = new Vector3(_targetX, _position.y, _position.z);
            Target.transform.position = _targetPos;
        }
        
        public void MoveVertical(float _targetY, TweenCallback _callback = null)
        {
            StopMove();

            var _position  = Target.transform.position;
            var _targetPos = new Vector3(_position.x, _targetY, _position.z);

            moveProcess = Target
                .DOMove(_targetPos, Duration)
                .SetEase(Ease.InOutExpo)
                .OnComplete(_callback);
        }

        public void MoveVerticalImmediate(float _targetY)
        {
            var _position  = Target.transform.position;
            var _targetPos = new Vector3(_position.x, _targetY, _position.z);
            Target.transform.position = _targetPos;
        }

        private void StopMove()
        {
            if (moveProcess != null)
            {
                moveProcess.Kill();
                moveProcess = null;
            }
        }
    }
}
