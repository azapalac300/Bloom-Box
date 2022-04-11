using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeIndicator : MonoBehaviour
{
     SpriteRenderer spriteRenderer;
    public Text textDisplay;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRed()
    {
        spriteRenderer.color = Color.red;
    }

    public void SetGreen()
    {
        spriteRenderer.color = Color.green;
    }

    public void SetText(string s)
    {
        textDisplay.text = s;
    }
}
