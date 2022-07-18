using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TouchControls : MonoBehaviour
{

    public static Vector3 mainTouchPos;

    public static event Action SwipeUpEvent;

    public static event Action SwipeDownEvent;

    public bool UsingTouch;

    private float prevTouchDelta;

    private const float touchDeltaThresh = 20f;
    public static float TouchDelta { get { return touchDelta; } }

    public static bool Touching { get { return Input.touchCount > 0;  } }

    private static float touchDelta;

    private Vector2 fingerStartPositionSwipe;
    private Vector2 fingerStartPositionScroll;


    private Vector2 fingerEndPositionSwipe;
    private Vector2 fingerEndPositionScroll;

    private Dictionary<int, Touch> touches;


    // Start is called before the first frame update
    void Start()
    {
        touches = new Dictionary<int, Touch>();
        touchDelta = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMainTouch();

        HandleSwipe();

        DetectTouchDelta();

    }


    public void HandleMainTouch()
    {

        for(int i = 0; i < Input.touchCount; i++)
        {
            if(Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if (!touches.ContainsKey(i))
                {
                    touches.Add(i, Input.GetTouch(i));
                }
            }


            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                if (touches.ContainsKey(i))
                {
                    touches.Remove(i);
                }
            }
        }


        if (UsingTouch)
        {
            if (Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(0);
                mainTouchPos = t.position;
            }
        }
        else
        {
            Vector3 mousePos = Input.mousePosition;
            mainTouchPos = mousePos;
        }
    }

    public void HandleSwipe()
    {
        if (Input.touchCount > 1) {
            for (int i = 1; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if(touch.phase == TouchPhase.Began)
                {
                    fingerStartPositionSwipe = touch.position;
                    fingerEndPositionSwipe = touch.position;
                }

                if(touch.phase == TouchPhase.Ended)
                {

                    fingerEndPositionSwipe = touch.position;
                    DetectSwipe();
                }

            }
        }
    }

    public void DetectSwipe()
    {
        //Fires a swipe event if there's a swipe
        //For now only detect a swipe up or swipe down event.

        Vector2 diffVector = fingerStartPositionSwipe - fingerEndPositionSwipe;

        string displayString = diffVector.ToString();

        
        if(diffVector.y > ResourceManager.minSwipeDist)
        {
            displayString += " Swiped Down!";
            SwipeDownEvent?.Invoke();
        }

        
        if ((-1*diffVector.y) > ResourceManager.minSwipeDist)
        {
            displayString += " Swiped Up!";
            SwipeUpEvent?.Invoke();
        }
        
    }

    public void DetectTouchDelta()
    {
        if(Touching)
        {
            prevTouchDelta = touchDelta;
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began )
            {
                fingerStartPositionScroll = touch.position;
                fingerEndPositionScroll = touch.position;
                touchDelta = 0;
            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Stationary)
            {
                fingerEndPositionScroll = touch.position;
                touchDelta = fingerEndPositionScroll.y - fingerStartPositionScroll.y;

            }

        }
        else
        {
            touchDelta = 0;
        }
    }
  
}
