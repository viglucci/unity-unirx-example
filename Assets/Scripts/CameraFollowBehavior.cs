using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBehavior : MonoBehaviour
{
    public Transform followTransform;
    public BoxCollider2D mapBounds;

    private float _xMin, _xMax, _yMin, _yMax;
    private float _cameraX, _cameraY;
    private float _camOrthSize;
    private float _camRatio;
    private Camera _mainCam;
    private const float SmoothSpeed = 0.5f;

    void Start()
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
        _cameraY = Mathf.Clamp(
            followPosition.y, 
            _yMin + _camOrthSize, 
            _yMax - _camOrthSize);
        
        _cameraX = Mathf.Clamp(
            followPosition.x, 
            _xMin + _camRatio, 
            _xMax - _camRatio);

        var existingCameraPos = transform.position;
        var nextCameraPos = new Vector3(_cameraX, _cameraY, existingCameraPos.z);
        var newCameraPosition = Vector3.Lerp(
            existingCameraPos,
            nextCameraPos,
            SmoothSpeed);
        
        Debug.Log((
            existingCameraPos,
            nextCameraPos
        ));
        
        transform.position = newCameraPosition;
    }
}
