using UniRx;
using UnityEngine;

public class PlayerSpriteControllerBehavior : MonoBehaviour
{
    public RuntimeAnimatorController idleDownAnimator;
    public RuntimeAnimatorController idleUpAnimator;
    public RuntimeAnimatorController idleSideAnimator;
    public RuntimeAnimatorController walkDownAnimator;
    public RuntimeAnimatorController walkUpAnimator;
    public RuntimeAnimatorController walkSideAnimator;

    private PlayerMovementControllerBehavior _movementController;

    void Start()
    {
        _movementController = gameObject.GetComponent<PlayerMovementControllerBehavior>();
        _movementController
            .MovementState
            .Subscribe(OnMovementStateUpdate);
    }

    private void OnMovementStateUpdate(MovementStatus nextStatus)
    {
        var animator = gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = nextStatus switch
        {
            MovementStatus.DownIdle => idleDownAnimator,
            MovementStatus.UpIdle => idleUpAnimator,
            MovementStatus.DownMove => walkDownAnimator,
            MovementStatus.UpMove => walkUpAnimator,
            MovementStatus.RightMove => walkSideAnimator,
            MovementStatus.LeftMove => walkSideAnimator,
            MovementStatus.RightIdle => idleSideAnimator,
            MovementStatus.LeftIdle => idleSideAnimator,
            _ => animator.runtimeAnimatorController
        };
        
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = nextStatus switch
        {
            MovementStatus.RightMove => true,
            MovementStatus.RightIdle => true,
            _ => false
        };
    }
}