using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtons : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menuButton, resetButton, score, playAreaTop;
   
    public void SetUpForTest()
    {
        menuButton.SetActive(false);
        resetButton.SetActive(false);
        playAreaTop.SetActive(false);
        score.SetActive(false);
    }

    public void SetUpForEdit()
    {
        menuButton.SetActive(true);
        resetButton.SetActive(false);
        playAreaTop.SetActive(false);
        score.SetActive(false);
    }

    public void SetUpForPuzzle()
    {
        menuButton.SetActive(true);
        resetButton.SetActive(true);
        playAreaTop.SetActive(false);
        score.SetActive(false);
    }

    public void SetUpForEndless()
    {
        menuButton.SetActive(true);
        resetButton.SetActive(false);
        playAreaTop.SetActive(true);
        score.SetActive(true);
    }
}
