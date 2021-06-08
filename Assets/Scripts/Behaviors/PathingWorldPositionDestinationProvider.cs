using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Behaviors
{
    public class PathingWorldPositionDestinationProvider : MonoBehaviour, IWorldPositionDestinationProvider
    {
        private int _currentPointIdx = -1;
        private readonly List<Vector3> _points = new List<Vector3>();

        private readonly Subject<Vector3> _destinations = new Subject<Vector3>();
        public IObservable<Vector3> WorldPositionUpdates => _destinations.AsObservable();

        public Path path;

        private void Start()
        {
            InitializePoints();
            
            IncrementToNextWorldPoint();
            
            SubscribeToDestinationStatusUpdates();
        }

        private void SubscribeToDestinationStatusUpdates()
        {
            gameObject.GetComponent<WorldPositionDestinationMovementBehavior>()
                .DestinationStatusUpdates
                .Where(status => status == DestinationStatus.Reached)
                .Subscribe(status => IncrementToNextWorldPoint());
        }

        private void InitializePoints()
        {
            var previous = transform.position;
            foreach (var checkpoint in path.checkPoints)
            {
                var point = new Vector3(
                    previous.x + checkpoint.x,
                    previous.y + checkpoint.y,
                    previous.z);
                _points.Add(point);
                previous = point;
            }
        }

        private void IncrementToNextWorldPoint()
        {
            _currentPointIdx += 1;
            if (_currentPointIdx > _points.Count - 1) _currentPointIdx = 0;
            var nextPoint = _points[_currentPointIdx];
            _destinations.OnNext(nextPoint);
        }
    }
}