using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchControls : MonoBehaviour
{
    public static Vector3 mainTouchPos;

    public static event Action SwipeUpEvent;

    public static event Action SwipeDownEvent;

    public bool UsingTouch;
    

    private Vector2 fingerStartPosition;

    private Vector2 fingerEndPosition;
    //public GameObject touchMarker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMainTouch();

        HandleSwipe();
    }


    public void HandleMainTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            Vector3 mousePos = Input.mousePosition;

            if (UsingTouch)
            {
                mainTouchPos = t.position;
            }
            else
            {
                mainTouchPos = mousePos;
            }
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


                if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Moved)
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

        if(diffVector.y> ResourceManager.minSwipeDist && Mathf.Abs(diffVector.x) < ResourceManager.diagonalTolerance)
        {
            Debug.Log("Swiped up!");
        }


        if ((-1*diffVector.y) > ResourceManager.minSwipeDist && Mathf.Abs(diffVector.x) < ResourceManager.diagonalTolerance)
        {
            Debug.Log("Swiped down!");
        }
    }

  
}
