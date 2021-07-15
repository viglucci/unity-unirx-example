using System;
using UniRx;
using UnityEngine;

namespace Behaviors
{
    public class IdentityWorldPositionUpdateProvider : MonoBehaviour, IWorldPositionDestinationProvider
    {
        private Vector3 _worldPosition;
    
        private readonly Subject<Vector3> _worldPositionUpdates = new Subject<Vector3>();

        public IObservable<Vector3> WorldPositionObservable => _worldPositionUpdates.AsObservable();

        public Vector3 WorldPosition {
            get => _worldPosition;
            set
            {
                _worldPosition = value;
                _worldPositionUpdates.OnNext(_worldPosition);
            }
        }
    }
}
