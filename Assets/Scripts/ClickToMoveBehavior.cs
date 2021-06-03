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
            // If the player clicks OR holds down the left mouse button
            .Where(_ => Input.GetMouseButtonUp(0) || Input.GetMouseButton(0));
        
        leftClicks.Subscribe(_ => OnLeftClickOrHold());
    }

    private void OnLeftClickOrHold()
    {
        MoveToMousePosition();
    }

    private void MoveToMousePosition()
    {
        var mousePos = new Vector3(
            Input.mousePosition.x, 
            Input.mousePosition.y, 
            0.0f);

        _nextPosition = _cam.ScreenToWorldPoint(mousePos);
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
