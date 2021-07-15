using System;
using UnityEngine;

namespace Behaviors
{
    public interface IWorldPositionDestinationProvider
    {
        Vector3 WorldPosition { get; set; }
        
        IObservable<Vector3> WorldPositionObservable { get; }
    }
}