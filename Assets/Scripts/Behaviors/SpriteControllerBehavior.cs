using System;
using UniRx;
using UnityEngine;

namespace Behaviors
{
    public class SpriteControllerBehavior : MonoBehaviour
    {
        private MovementStatus _currentMovementStatus;
        private Orientation _currentOrientation;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
    
        public RuntimeAnimatorController idleDownAnimator;
        public RuntimeAnimatorController idleUpAnimator;
        public RuntimeAnimatorController idleSideAnimator;
        public RuntimeAnimatorController walkDownAnimator;
        public RuntimeAnimatorController walkUpAnimator;
        public RuntimeAnimatorController walkSideAnimator;

        private void Start()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            _animator = gameObject.GetComponent<Animator>();
            var orientationProvider = gameObject.GetComponent<OrientationStateProviderBehavior>();
            var movementController = gameObject.GetComponent<WorldPositionDestinationMovementBehavior>();

            _currentMovementStatus = movementController.MovementStatus;
            _currentOrientation = orientationProvider.Orientation;
        
            orientationProvider.OrientationUpdates
                .Subscribe(OnOrientationUpdate);
        
            movementController.MovementStatusUpdates
                .Subscribe(OnMovementStatusUpdate);
        
            UpdateSprite();
        }

        private void OnOrientationUpdate(Orientation orientation)
        {
            _currentOrientation = orientation;
            UpdateSprite();
        }

        private void OnMovementStatusUpdate((MovementStatus, MovementStatus current) update)
        {
            _currentMovementStatus = update.current;
            UpdateSprite();
        }
    
        private void UpdateSprite()
        {
            var orientation = _currentOrientation;
            var isIdle = _currentMovementStatus == MovementStatus.Idle;

            if (isIdle)
            {
                _animator.runtimeAnimatorController = orientation switch
                {
                    Orientation.Up => idleUpAnimator,
                    Orientation.Right => idleSideAnimator,
                    Orientation.Down => idleDownAnimator,
                    Orientation.Left => idleSideAnimator,
                    _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
                };
            }
            else
            {
                _animator.runtimeAnimatorController = orientation switch
                {
                    Orientation.Up => walkUpAnimator,
                    Orientation.Right => walkSideAnimator,
                    Orientation.Down => walkDownAnimator,
                    Orientation.Left => walkSideAnimator,
                    _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
                };
            }

            _spriteRenderer.flipX = orientation switch
            {
                Orientation.Right => true,
                _ => false
            };
        }
    }
}