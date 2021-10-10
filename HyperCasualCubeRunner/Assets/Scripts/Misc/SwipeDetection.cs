using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private UnityEvent evSwipeUp = null;
    [SerializeField] private UnityEvent evSwipeDown = null;
    [SerializeField] private UnityEvent evSwipeLeft = null;
    [SerializeField] private UnityEvent evSwipeRight = null;

    private bool isFingerDown = false;
    private Vector2 startPos = new Vector2();
    [SerializeField] private float distToDetect = 20.0f;

    [Tooltip("Allow the use of mouse to simulate swipe on PC.\n" +
         " Using mouse disables the future use of handling multitouches.\n" +
         " The use of mouse button should be disabled for production builds (for mutitouch scenarios).")]
    [SerializeField] private bool useMouseButton = true;

    private void Update()
    {
        if (useMouseButton)
            MouseInput();
        else
            TouchInput();
    }


    private void TouchInput()
    {
        //  New touch:
        if (!isFingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            startPos = Input.touches[0].position;
            isFingerDown = true;
        }

        //  Swipes:
        if (isFingerDown)
        {
            //  Swipe up:
            if (Input.touches[0].position.y >= (startPos.y + distToDetect))
            {
                isFingerDown = false;
                if (evSwipeUp != null)
                    evSwipeUp.Invoke();
            }
            //  Swipe down:
            else if (Input.touches[0].position.y <= (startPos.y - distToDetect))
            {
                isFingerDown = false;
                if (evSwipeUp != null)
                    evSwipeUp.Invoke();
            }
            //  Swipe left:
            else if (Input.touches[0].position.x <= (startPos.x - distToDetect))
            {
                isFingerDown = false;
                if (evSwipeLeft != null)
                    evSwipeLeft.Invoke();
            }
            //  Swipe right:
            else if (Input.touches[0].position.x >= (startPos.x + distToDetect))
            {
                isFingerDown = false;
                if (evSwipeRight != null)
                    evSwipeRight.Invoke();
            }
        }

        //  Released:
        if (isFingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            isFingerDown = false;
        }
    }

    private void MouseInput()
    {
        //  New click:
        if (!isFingerDown && Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            isFingerDown = true;
        }

        //  Swipes:
        if (isFingerDown)
        {
            //  Swipe up:
            if (Input.mousePosition.y >= (startPos.y + distToDetect))
            {
                isFingerDown = false;
                if (evSwipeUp != null)
                    evSwipeUp.Invoke();
            }
            //  Swipe down:
            else if (Input.mousePosition.y <= (startPos.y - distToDetect))
            {
                isFingerDown = false;
                if (evSwipeDown != null)
                    evSwipeDown.Invoke();
            }
            //  Swipe left:
            else if (Input.mousePosition.x <= (startPos.x - distToDetect))
            {
                isFingerDown = false;
                if (evSwipeLeft != null)
                    evSwipeLeft.Invoke();
            }
            //  Swipe right:
            else if (Input.mousePosition.x >= (startPos.x + distToDetect))
            {
                isFingerDown = false;
                if (evSwipeRight != null)
                    evSwipeRight.Invoke();
            }
        }

        //  Released:
        if (isFingerDown && Input.GetMouseButtonUp(0))
        {
            isFingerDown = false;
        }
    }

    public void OnSwipeUp()
    {
        Debug.Log("Up");
    }
    public void OnSwipeDown()
    {
        Debug.Log("Down");
    }
}
