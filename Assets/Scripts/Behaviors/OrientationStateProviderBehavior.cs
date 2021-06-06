using System;
using UniRx;
using UnityEngine;

namespace Behaviors
{
    public class OrientationStateProviderBehavior : MonoBehaviour
    {
        private readonly Subject<Orientation> _orientationUpdates = new Subject<Orientation>();

        [SerializeField]
        private Orientation initialOrientation;
    
        public Orientation Orientation { get; private set; }

        public IObservable<Orientation> OrientationUpdates => _orientationUpdates.AsObservable();
    
        private void Awake()
        {
            Orientation = initialOrientation;
        
            gameObject
                .GetComponent<WorldPositionDestinationMovementBehavior>()
                .PositionUpdates
                .Subscribe(OnPositionUpdate);
        }

        private void OnPositionUpdate((Vector2, Vector2) update)
        {
            var (previousPosition, newPosition) = update;
            var direction = (newPosition - previousPosition).normalized;
            var xDelta = Math.Abs(direction.x);
            var yDelta = Math.Abs(direction.y);

            Orientation next;

            if (yDelta > xDelta)
            {
                // up or down
                next = direction.y >= 0f
                    ? Orientation.Up
                    : Orientation.Down;
            }
            else
            {
                // left or right
                next = direction.x >= 0f
                    ? Orientation.Right
                    : Orientation.Left;
            }

            _orientationUpdates.OnNext(next);
        }
    }
}
