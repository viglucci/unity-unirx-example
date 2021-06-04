using System;
using DefaultNamespace;
using UniRx;
using UnityEngine;

public class PlayerMovementControllerBehavior : MonoBehaviour
{
    private const float Speed = 5.0f;
    private Vector2 _nextPosition;
    private Camera _cam;
    private PlayerMovementState _currentMovementState = PlayerMovementState.DownIdle;
    private readonly Subject<PlayerMovementState> _movementUpdates = new Subject<PlayerMovementState>();

    public IObservable<PlayerMovementState> MovementState =>
        _movementUpdates.AsObservable();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ClickToMoveBehavior.Start");

        _cam = Camera.main;

        var leftClicks = Observable
            .EveryUpdate()
            // If the player clicks OR holds down the left mouse button
            .Where(_ => Input.GetMouseButtonUp(0) || Input.GetMouseButton(0));

        leftClicks.Subscribe(_ => OnLeftClickOrHold());
    }

    private void OnLeftClickOrHold()
    {
        MoveToMousePosition();
    }

    private void MoveToMousePosition()
    {
        var mousePos = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            0.0f);

        _nextPosition = _cam.ScreenToWorldPoint(mousePos);
        
        var nextState = DetermineNextMovementState();
        if (nextState == _currentMovementState) return;
        
        _currentMovementState = nextState;
        _movementUpdates.OnNext(nextState);
    }

    private PlayerMovementState DetermineNextMovementState()
    {
        var nextState = _currentMovementState;
        var hasReachedDestination = HasReachedDestination();
        switch (_currentMovementState)
        {
            case PlayerMovementState.DownMove when hasReachedDestination:
                nextState = PlayerMovementState.DownIdle;
                break;
            case PlayerMovementState.UpMove when hasReachedDestination:
                nextState = PlayerMovementState.UpIdle;
                break;
            default:
            {
                if (_nextPosition.y > transform.position.y)
                {
                    nextState = PlayerMovementState.UpMove;
                }
                else if (_nextPosition.y < transform.position.y)
                {
                    nextState = PlayerMovementState.DownMove;
                }

                break;
            }
        }

        return nextState;
    }

    // Update is called once per frame
    void Update()
    {
        if (!HasReachedDestination())
        {
            MoveTowardsNextPosition();
        }
        else if (_currentMovementState != PlayerMovementState.DownIdle &&
                 _currentMovementState != PlayerMovementState.UpIdle)
        {
            // We reached our destination so we need to update our state
            UpdateStationaryMovementState();
        }
    }

    private bool HasReachedDestination()
    {
        return Vector2.Distance(transform.position, _nextPosition) == 0;
    }

    private void MoveTowardsNextPosition()
    {
        var position = transform.position;
        var step = Speed * Time.deltaTime;
        position = Vector2.MoveTowards(position, _nextPosition, step);
        transform.position = position;
    }

    private void UpdateStationaryMovementState()
    {
        switch (_currentMovementState)
        {
            case PlayerMovementState.RightIdle:
                break;
            case PlayerMovementState.LeftIdle:
                break;
            case PlayerMovementState.DownMove:
                _currentMovementState = PlayerMovementState.DownIdle;
                break;
            case PlayerMovementState.UpMove:
                _currentMovementState = PlayerMovementState.UpIdle;
                break;
        }

        _movementUpdates.OnNext(_currentMovementState);
    }
}