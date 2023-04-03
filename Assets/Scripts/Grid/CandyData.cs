using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CandyData 
{
    [HideInInspector]
    public int Vscore, Hscore;
    [HideInInspector]
    public CandyColor candyColor;
    

    

    public CandyData(int vScore, int hScore, CandyColor color)
    {
        Vscore = vScore;
        Hscore = hScore;
        candyColor = color;
    }

}

public enum CandyColor
{
    red,
    blue,
    green,
    yellow
}
