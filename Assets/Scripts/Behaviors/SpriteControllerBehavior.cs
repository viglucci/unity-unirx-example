using System;
using UniRx;
using UnityEngine;

namespace Behaviors
{
    public class SpriteControllerBehavior : MonoBehaviour
    {
        private MovementStatus _currentMovementStatus;
        private Orientation _currentOrientation;
    
        public RuntimeAnimatorController idleDownAnimator;
        public RuntimeAnimatorController idleUpAnimator;
        public RuntimeAnimatorController idleSideAnimator;
        public RuntimeAnimatorController walkDownAnimator;
        public RuntimeAnimatorController walkUpAnimator;
        public RuntimeAnimatorController walkSideAnimator;

        private void Start()
        {
            var orientationProvider = gameObject.GetComponent<OrientationStateProviderBehavior>();
            var movementController = gameObject.GetComponent<PlayerMovementControllerBehavior>();

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

        private void OnMovementStatusUpdate((MovementStatus, MovementStatus) update)
        {
            var current = update.Item2;
            _currentMovementStatus = current;
            UpdateSprite();
        }
    
        private void UpdateSprite()
        {
            var orientation = _currentOrientation;
            var isIdle = _currentMovementStatus == MovementStatus.Idle;
            var animator = gameObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = orientation switch
            {
                Orientation.Up => isIdle ? idleUpAnimator : walkUpAnimator,
                Orientation.Right => isIdle ? idleSideAnimator : walkSideAnimator,
                Orientation.Down => isIdle ? idleDownAnimator : walkDownAnimator,
                Orientation.Left => isIdle ? idleSideAnimator : walkSideAnimator,
                _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
            };

            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = orientation switch
            {
                Orientation.Right => true,
                _ => false
            };
        }
    }
}