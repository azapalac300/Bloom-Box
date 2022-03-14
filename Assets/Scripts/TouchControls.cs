using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchControls : MonoBehaviour
{
    public SwipeIndicator swipeIndicator;

    public static Vector3 mainTouchPos;

    public static event Action SwipeUpEvent;

    public static event Action SwipeDownEvent;

    public bool UsingTouch;


    private Vector2 fingerStartPosition;

    private Vector2 fingerEndPosition;

    private Dictionary<int, Touch> touches;


    // Start is called before the first frame update
    void Start()
    {
        touches = new Dictionary<int, Touch>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMainTouch();

        HandleSwipe();
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
                    fingerStartPosition = touch.position;
                    fingerEndPosition = touch.position;
                }


                if(touch.phase == TouchPhase.Ended)
                {
                    fingerEndPosition = touch.position;
                    DetectSwipe();
                }

            }
        }
    }

    public void DetectSwipe()
    {
        //Fires a swipe event if there's a swipe
        //For now only detect a swipe up or swipe down event.

        Vector2 diffVector = fingerStartPosition - fingerEndPosition;

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
        
        swipeIndicator.SetText(displayString);
    }

  
}
