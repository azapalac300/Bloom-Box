using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchControls : MonoBehaviour
{
    public static Vector3 mainTouchPos;

    public static event Action SwipeUpEvent;

    public static event Action SwipeDownEvent;

    //public GameObject touchMarker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMainTouch();

        HandleSwipeUp();

        HandleSwipeDown();
    }


    public void HandleMainTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            Vector3 mousePos = Input.mousePosition;
            mainTouchPos = t.position;
        }
    }

    public void HandleSwipeUp()
    {
        
    }

    public void HandleSwipeDown()
    {

    }
}
