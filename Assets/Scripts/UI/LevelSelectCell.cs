using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectCell : MonoBehaviour
{
    public CellColor color;
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        image.color = Cell.SelectColor(color);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
