using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public CellColor color;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Color c = Color.white;
        switch (color)
        {
            //Wild and Dead both do something special when the sprite is updated

            case CellColor.red:

                ColorUtility.TryParseHtmlString("#ff3300", out c);
                break;

            case CellColor.orange:
                ColorUtility.TryParseHtmlString("#fdad3f", out c);
                break;


            case CellColor.cyan:
                ColorUtility.TryParseHtmlString("#00E3E5", out c);
                break;


            case CellColor.green:
                ColorUtility.TryParseHtmlString("#00CB00", out c);
                break;

            case CellColor.blue:
                ColorUtility.TryParseHtmlString("#1A68FF", out c);
                break;

            case CellColor.purple:
                ColorUtility.TryParseHtmlString("#dea3ff", out c);
                break;

            case CellColor.dead:
                ColorUtility.TryParseHtmlString("#7B7B7B", out c);
                break;

        }

        spriteRenderer.color = c;
    }
}
