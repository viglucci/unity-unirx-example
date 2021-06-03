using System;
using UniRx;
using UnityEngine;

public class ClickToMoveBehavior : MonoBehaviour
{
    private const float Speed = 5.0f;
    private Vector2 _nextPosition;
    private Camera _cam;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ClickToMoveBehavior.Start");
        
        _cam = Camera.main;
        _nextPosition = transform.position;
        
        var leftClicks = Observable
            .EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0));

        var doubleClicks = leftClicks
            .Buffer(leftClicks.Throttle(TimeSpan.FromMilliseconds(250)))
            .Select(xs => xs.Count)
            .Where(count => count >= 2);
        
        doubleClicks.Subscribe(_ => OnDoubleClick());
    }

    private void OnDoubleClick()
    {
        Debug.Log("DoubleClick Detected!");

        MoveToMousePosition();
    }

    private void MoveToMousePosition()
    {
        var mousePos = new Vector2
        {
            x = Input.mousePosition.x,
            y = Input.mousePosition.y
        };

        _nextPosition = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        var step = Speed * Time.deltaTime;
        position = Vector2.MoveTowards(position, _nextPosition, step);
        transform.position = position;
    }
}
