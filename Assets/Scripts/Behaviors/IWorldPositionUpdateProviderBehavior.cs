using System;
using UnityEngine;

namespace Behaviors
{
    public interface IWorldPositionUpdateProviderBehavior
    {
        IObservable<Vector3> WorldPositionUpdates { get; }
    }
}