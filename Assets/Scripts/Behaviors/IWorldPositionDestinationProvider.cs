using System;
using UnityEngine;

namespace Behaviors
{
    public interface IWorldPositionDestinationProvider
    {
        IObservable<Vector3> WorldPositionUpdates { get; }
    }
}