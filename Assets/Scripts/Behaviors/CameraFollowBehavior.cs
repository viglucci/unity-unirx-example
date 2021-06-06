using UnityEngine;

namespace Behaviors
{
    public class CameraFollowBehavior : MonoBehaviour
    {
        public Transform followTransform;
        public BoxCollider2D mapBounds;

        private float _xMin, _xMax, _yMin, _yMax;
        private float _camOrthSize;
        private float _camRatio;
        private Camera _mainCam;
        private const float SmoothSpeed = 0.5f;

        private void Awake()
        {
            var bounds = mapBounds.bounds;
            _xMin = bounds.min.x;
            _xMax = bounds.max.x;
            _yMin = bounds.min.y;
            _yMax = bounds.max.y;
        
            _mainCam = GetComponent<Camera>();
            _camOrthSize = _mainCam.orthographicSize;
            _camRatio = (_xMax + _camOrthSize) / 2.0f;
        }
    
        private void Update()
        {
            var followPosition = followTransform.position;
        
            // Don't allow Y to exceed past collider area
            var cameraY = Mathf.Clamp(
                followPosition.y, 
                _yMin + _camOrthSize, 
                _yMax - _camOrthSize);
        
            // Don't allow X to exceed past collider area
            var cameraX = Mathf.Clamp(
                followPosition.x, 
                _xMin + _camRatio, 
                _xMax - _camRatio);

            var existingCameraPos = transform.position;
            var nextCameraPos = new Vector3(cameraX, cameraY, existingCameraPos.z);
            var newCameraPosition = Vector3.Lerp(
                existingCameraPos,
                nextCameraPos,
                SmoothSpeed);

            transform.position = newCameraPosition;
        }
    }
}
