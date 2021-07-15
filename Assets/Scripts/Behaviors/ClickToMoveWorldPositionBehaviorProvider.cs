using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviors
{
    public class ClickToMoveWorldPositionBehaviorProvider : MonoBehaviour
    {
        private Camera _camera;
    
        void Awake()
        {
            _camera = Camera.main;

            var player = gameObject.GetComponent<Core>().Player;

            var playerWorldPositionProvider = player
                .GetComponent<IWorldPositionDestinationProvider>();

            Observable
                .EveryUpdate()
                // If the player clicks OR holds down the left mouse button
                .Where(_ => Input.GetMouseButtonUp(0) || Input.GetMouseButton(0))
                // Filter out clicks that are on another object (such as UI buttons)
                .Where(_ => EventSystem.current.currentSelectedGameObject == null) 
                // React to click events
                .Subscribe(_ =>
                {
                    var mousePos = new Vector3(
                        Input.mousePosition.x,
                        Input.mousePosition.y,
                        0.0f);

                    var nextValue = _camera.ScreenToWorldPoint(mousePos);

                    playerWorldPositionProvider.WorldPosition = nextValue;
                });
        }
    }
}