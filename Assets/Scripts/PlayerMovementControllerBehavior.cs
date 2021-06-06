using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerMovementControllerBehavior : MonoBehaviour
{
    private const float Speed = 5.0f;
    private Vector3 _nextPosition;
    private MovementStatus _currentMovementStatus = MovementStatus.DownIdle;
    private readonly Subject<MovementStatus> _movementUpdates = new Subject<MovementStatus>();
    private readonly Subject<Vector2> _moveTowardsUpdates = new Subject<Vector2>();

    public IObservable<MovementStatus> MovementState =>
        _movementUpdates.AsObservable();

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("ClickToMoveBehavior.Start");
        
        SubscribeToMovementInput();

        // _movementUpdates.Subscribe(value => Debug.Log(value));

        _moveTowardsUpdates.Subscribe(
            nextPos => transform.position = nextPos);

        //TODO: dont allow movement off of the "map"
    }

    private void SubscribeToMovementInput()
    {
        var controller = gameObject.GetComponent<MouseMovementInputController>();
        var mouseInputs = controller.MouseInputs;
        mouseInputs.Subscribe(AssignNextPosition);
    }

    private void AssignNextPosition(Vector3 worldPosition)
    {
        _nextPosition = worldPosition;
        UpdateMovementState();
    }

    private void UpdateMovementState()
    {
        MovementStatus nextStatus;
        var hasReachedDestination = HasReachedDestination();
        switch (_currentMovementStatus)
        {
            case MovementStatus.DownMove when hasReachedDestination:
                nextStatus = MovementStatus.DownIdle;
                break;
            case MovementStatus.UpMove when hasReachedDestination:
                nextStatus = MovementStatus.UpIdle;
                break;
            default:
            {
                var position = transform.position;
                var direction = (_nextPosition - position).normalized;
                var xDelta = Math.Abs(direction.x);
                var yDelta = Math.Abs(direction.y);

                if (yDelta > xDelta)
                {
                    // up or down
                    nextStatus = direction.y >= 0f 
                        ? MovementStatus.UpMove 
                        : MovementStatus.DownMove;
                }
                else
                {
                    // left or right
                    nextStatus = direction.x >= 0f 
                        ? MovementStatus.RightMove 
                        : MovementStatus.LeftMove;
                }

                break;
            }
        }

        if (nextStatus == _currentMovementStatus) return;

        _currentMovementStatus = nextStatus;
        _movementUpdates.OnNext(nextStatus);
    }

    private void Update()
    {
        if (HasReachedDestination())
        {
            // We reached our destination so we need to update our state
            UpdateAndEmitStationaryMovementState();
        }
        else
        {
            MoveTowardsNextPosition();
        }
    }

    private bool HasReachedDestination()
    {
        return Vector2.Distance(transform.position, _nextPosition) == 0;
    }

    private void MoveTowardsNextPosition()
    {
        var currentPosition = transform.position;
        var step = Speed * Time.deltaTime;
        var nextPosition = Vector2.MoveTowards(currentPosition, _nextPosition, step);
        _moveTowardsUpdates.OnNext(nextPosition);
    }

    private void UpdateAndEmitStationaryMovementState()
    {
        var previousMovementState = _currentMovementStatus;

        _currentMovementStatus = _currentMovementStatus switch
        {
            MovementStatus.RightMove => MovementStatus.RightIdle,
            MovementStatus.LeftMove => MovementStatus.LeftIdle,
            MovementStatus.DownMove => MovementStatus.DownIdle,
            MovementStatus.UpMove => MovementStatus.UpIdle,
            _ => _currentMovementStatus
        };

        if (previousMovementState == _currentMovementStatus) return;

        _movementUpdates.OnNext(_currentMovementStatus);
    }
}