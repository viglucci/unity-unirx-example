using System;
using UniRx;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class MouseMovementInputController : MonoBehaviour
{
    private IObservable<Vector3> _mouseInputs;
    public IObservable<Vector3> MouseInputs => _mouseInputs;

    // Start is called before the first frame update
    void Start()
    {
        _mouseInputs = Observable
            .EveryUpdate()
            // If the player clicks OR holds down the left mouse button
            .Where(_ => Input.GetMouseButtonUp(0) || Input.GetMouseButton(0))
            .Select(_ =>
            {
                var mousePos = new Vector3(
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                    0.0f);

                Debug.Assert(Camera.main != null, "Camera.main != null");
                
                return Camera.main.ScreenToWorldPoint(mousePos);
            });
    }
}