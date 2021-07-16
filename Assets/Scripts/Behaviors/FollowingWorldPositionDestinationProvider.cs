using System;
using System.Collections;
using System.Collections.Generic;
using Behaviors;
using UniRx;
using UnityEngine;

public class FollowingWorldPositionDestinationProvider : MonoBehaviour, IWorldPositionDestinationProvider
{
    private readonly Subject<Vector3> _destinations = new Subject<Vector3>();
    
    public Vector3 WorldPosition { get; set; }

    public IObservable<Vector3> WorldPositionObservable => _destinations.AsObservable();

    // Start is called before the first frame update
    void Start()
    {
        var core = FindObjectOfType<Core>();
        var player = core.GetObjectById(1);
        player.GetComponent<IWorldPositionDestinationProvider>()
            .WorldPositionObservable
            .Subscribe((Vector3 worldPosition) =>
            {
                Debug.Log("Moving to next player position");
                _destinations.OnNext(worldPosition);
            });
    }
}