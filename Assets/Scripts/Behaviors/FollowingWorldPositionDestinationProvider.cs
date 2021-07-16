using System;
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
            .Subscribe((Vector3 target) =>
            {
                var offsetTarget = CalculateOffsetTarget(target);
                _destinations.OnNext(offsetTarget);
            });
    }

    private Vector3 CalculateOffsetTarget(Vector3 target)
    {
        var direction = (target - gameObject.transform.position).normalized;
        var xDelta = Math.Abs(direction.x);
        var yDelta = Math.Abs(direction.y);

        Vector3 offsetTarget;

        if (yDelta > xDelta)
        {
            // target is above
            if (direction.y >= 0f)
            {
                offsetTarget = new Vector3(
                    target.x,
                    target.y - 1,
                    target.z
                );
            }
            // target is below
            else
            {
                offsetTarget = new Vector3(
                    target.x,
                    target.y + 1,
                    target.z
                );
            }
        }
        else
        {
            // target is left
            if (direction.x >= 0f)
            {
                offsetTarget = new Vector3(
                    target.x - 1,
                    target.y,
                    target.z
                );
            }
            // target is right
            else
            {
                offsetTarget = new Vector3(
                    target.x + 1,
                    target.y,
                    target.z
                );
            }
        }

        return offsetTarget;
    }
}