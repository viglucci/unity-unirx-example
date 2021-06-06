using System;
using UniRx;
using UnityEngine;

public class PlayerMovementControllerBehavior : MonoBehaviour
{
    private MovementStatus _movementStatus = MovementStatus.Idle;

    private const float Speed = 5.0f;

    private Vector2 _destinationPosition;

    private readonly Subject<(Vector2, Vector2)> _positionUpdates = new Subject<(Vector2, Vector2)>();

    private readonly Subject<(MovementStatus, MovementStatus)> _movementStatusUpdates =
        new Subject<(MovementStatus, MovementStatus)>();

    public IObservable<(Vector2, Vector2)> PositionUpdates => _positionUpdates.AsObservable();
    public IObservable<(MovementStatus, MovementStatus)> MovementStatusUpdates => _movementStatusUpdates.AsObservable();
    public MovementStatus MovementStatus => _movementStatus;

    private void Awake()
    {
        _destinationPosition = transform.position;
        SubscribeToMovementInput();
    }

    private void Update()
    {
        var nextPosition = GetLerpTowards(_destinationPosition);
        var previousPosition = (Vector2) transform.position;
        if (Equals(previousPosition, nextPosition))
        {
            UpdateMovementStatus(MovementStatus.Idle);
            return;
        }

        transform.position = nextPosition;
        _positionUpdates.OnNext((previousPosition, nextPosition));
        UpdateMovementStatus(MovementStatus.Moving);
    }

    private void UpdateMovementStatus(MovementStatus next)
    {
        if (_movementStatus == next) return;
        var previousMovementStatus = _movementStatus;
        _movementStatus = next;
        _movementStatusUpdates.OnNext((previousMovementStatus, next));
    }

    private void SubscribeToMovementInput()
    {
        var controller = gameObject.GetComponent<MouseMovementInputController>();
        var mouseInputs = controller.MouseInputs;
        mouseInputs.Subscribe(OnMouseMovementInput);
    }

    private void OnMouseMovementInput(Vector3 worldPosition)
    {
        _destinationPosition = worldPosition;
    }

    private Vector2 GetLerpTowards(Vector2 worldPosition)
    {
        var position = transform.position;
        var step = Speed * Time.deltaTime;
        return Vector2.MoveTowards(position, worldPosition, step);
    }
}