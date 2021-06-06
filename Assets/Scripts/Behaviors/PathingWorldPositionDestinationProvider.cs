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

        private void Start()
        {
            gameObject.GetComponent<WorldPositionDestinationMovementBehavior>()
                .DestinationStatusUpdates
                .Subscribe(status =>
                {
                    if (status != DestinationStatus.Reached) return;
                    IncrementToNextWorldPoint();
                });

            InitializePoints();

            IncrementToNextWorldPoint();
        }

        private void InitializePoints()
        {
            var initial = transform.position;

            var pointOne = new Vector3()
            {
                x = initial.x,
                y = initial.y + 3,
                z = initial.z
            };
            
            var pointTwo = new Vector3()
            {
                x = pointOne.x - 6,
                y = pointOne.y,
                z = pointOne.z
            };
            
            var pointThree = new Vector3()
            {
                x = pointTwo.x,
                y = pointTwo.y - 6,
                z = pointTwo.z
            };
            
            var pointFour = new Vector3()
            {
                x = pointThree.x + 6,
                y = pointThree.y,
                z = pointThree.z
            };
            
            _points.AddRange(new List<Vector3>()
            {
                pointOne, pointTwo, pointThree, pointFour
            });
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