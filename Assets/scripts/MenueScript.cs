using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenueScript : MonoBehaviour
{
    public Canvas Start;
    public Canvas Name;
    public Canvas Chara;
    public Canvas TeamMode;
    public Canvas Play;

    private void Awake()
    {
        setscreen(0);
    }
    public void setscreen(int set)
    {
        Start.enabled = false;
        Name.enabled = false;
        Chara.enabled = false;
        TeamMode.enabled = false;
        Play.enabled = false;
        if (set == 0) Start.enabled = true;
        if (set == 1) Name.enabled = true;
        if (set == 2) Chara.enabled = true;
        if (set == 3) TeamMode.enabled = true;
        if (set == 4) Play.enabled = true;
        
    }
}
