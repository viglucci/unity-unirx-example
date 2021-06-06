using System;
using UniRx;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Behaviors
{
    public class ClickToMoveWorldPositionDestinationProvider : MonoBehaviour, IWorldPositionDestinationProvider
    {
        private readonly Subject<Vector3> _mouseInputs = new Subject<Vector3>();
        public IObservable<Vector3> WorldPositionUpdates => _mouseInputs.AsObservable();

        void Awake()
        {
            Observable
                .EveryUpdate()
                // If the player clicks OR holds down the left mouse button
                .Where(_ => Input.GetMouseButtonUp(0) || Input.GetMouseButton(0))
                .Subscribe(_ =>
                {
                    var mousePos = new Vector3(
                        Input.mousePosition.x,
                        Input.mousePosition.y,
                        0.0f);

                    Debug.Assert(Camera.main != null, "Camera.main != null");

                    var nextValue = Camera.main.ScreenToWorldPoint(mousePos);

                    _mouseInputs.OnNext(nextValue);
                });
        }
    }
}