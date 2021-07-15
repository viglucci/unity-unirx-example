using System;
using UniRx;
using UnityEngine;

namespace Behaviors
{
    public class WorldPositionDestinationMovementBehavior : MonoBehaviour
    {
        private const float Speed = 5.0f;

        private Vector2 _destinationPosition;

        private DestinationStatus _destinationStatus = DestinationStatus.Reached;

        private readonly Subject<(Vector2, Vector2)> _positionUpdates = new Subject<(Vector2, Vector2)>();
        private readonly Subject<DestinationStatus> _destinationStatusUpdates = new Subject<DestinationStatus>();

        private readonly Subject<(MovementStatus, MovementStatus)> _movementStatusUpdates =
            new Subject<(MovementStatus, MovementStatus)>();

        public IObservable<(Vector2, Vector2)> PositionUpdates => _positionUpdates.AsObservable();

        public IObservable<(MovementStatus, MovementStatus)> MovementStatusUpdates =>
            _movementStatusUpdates.AsObservable();

        public IObservable<DestinationStatus> DestinationStatusUpdates =>
            _destinationStatusUpdates.AsObservable();

        public MovementStatus MovementStatus { get; private set; } = MovementStatus.Idle;

        private void Awake()
        {
            _destinationPosition = transform.position;
            SubscribeToDestinationUpdates();
        }

        private void Update()
        {
            var nextPosition = GetLerpTowards(_destinationPosition);
            var previousPosition = (Vector2) transform.position;
            if (Equals(previousPosition, nextPosition))
            {
                UpdateMovementStatus(MovementStatus.Idle);
                UpdateDestinationStatus(DestinationStatus.Reached);
                return;
            }

            transform.position = nextPosition;
            _positionUpdates.OnNext((previousPosition, nextPosition));
            UpdateMovementStatus(MovementStatus.Moving);
        }

        private void UpdateDestinationStatus(DestinationStatus next)
        {
            if (_destinationStatus == next) return;
            _destinationStatus = next;
            _destinationStatusUpdates.OnNext(_destinationStatus);
        }

        private void UpdateMovementStatus(MovementStatus next)
        {
            if (MovementStatus == next) return;
            var previousMovementStatus = MovementStatus;
            MovementStatus = next;
            _movementStatusUpdates.OnNext((previousMovementStatus, next));
        }

        private void SubscribeToDestinationUpdates()
        {
            gameObject
                .GetComponent<IWorldPositionDestinationProvider>()
                .WorldPositionObservable
                .Subscribe(OnDestinationUpdate);
        }

        private void OnDestinationUpdate(Vector3 worldPosition)
        {
            _destinationPosition = worldPosition;
            UpdateDestinationStatus(DestinationStatus.Unreached);
        }

        private Vector2 GetLerpTowards(Vector2 worldPosition)
        {
            var position = transform.position;
            var step = Speed * Time.deltaTime;
            return Vector2.MoveTowards(position, worldPosition, step);
        }
    }
}