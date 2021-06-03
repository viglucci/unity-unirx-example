using DefaultNamespace;
using UniRx;
using UnityEngine;

public class PlayerSpriteControllerBehavior : MonoBehaviour
{
    public RuntimeAnimatorController idleDownAnimator;
    public RuntimeAnimatorController idleUpAnimator;
    public RuntimeAnimatorController walkDownAnimator;
    public RuntimeAnimatorController walkUpAnimator;

    private PlayerMovementControllerBehavior _movementController;

    void Start()
    {
        _movementController = gameObject.GetComponent<PlayerMovementControllerBehavior>();
        _movementController
            .MovementState
            .Subscribe(OnMovementStateUpdate);
    }

    private void OnMovementStateUpdate(PlayerMovementState nextState)
    {
        var animator = gameObject.GetComponent<Animator>();
        switch (nextState)
        {
            case PlayerMovementState.DownIdle:
                animator.runtimeAnimatorController = idleDownAnimator;
                break;
            case PlayerMovementState.UpIdle:
                animator.runtimeAnimatorController = idleUpAnimator;
                break;
            case PlayerMovementState.DownMove:
                animator.runtimeAnimatorController = walkDownAnimator;
                break;
            case PlayerMovementState.UpMove:
                animator.runtimeAnimatorController = walkUpAnimator;
                break;
        }
    }
}