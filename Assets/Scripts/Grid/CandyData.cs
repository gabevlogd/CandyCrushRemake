using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CandyData 
{
    //[HideInInspector]
    public int Vscore, Hscore;
    public bool AlreadyAdded;
    [HideInInspector]
    public CandyColor candyColor;
    [HideInInspector]
    public Candy TriggeredBy; //DEVO ANCORA CAPIRE SE SERVE




    public CandyData(int vScore, int hScore, CandyColor color)
    {
        Vscore = vScore;
        Hscore = hScore;
        candyColor = color;
        AlreadyAdded = false;
    }

}

public enum CandyColor
{
    red,
    blue,
    green,
    yellow
}
