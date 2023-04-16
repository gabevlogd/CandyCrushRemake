using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CandyData 
{
    public int Vscore, Hscore;
    public bool AlreadyAdded;
    public CandyColor candyColor;
    public Candy TriggeredBy; //DEVO ANCORA CAPIRE SE SERVE




    public CandyData(int vScore, int hScore, CandyColor color)
    {
        Vscore = vScore;
        Hscore = hScore;
        candyColor = color;
        AlreadyAdded = false;
    }

    public void ResetData()
    {
        Vscore = 0;
        Hscore = 0;
        AlreadyAdded = false;
        TriggeredBy = null;
    }

}

public enum CandyColor
{
    red,
    blue,
    green,
    yellow,
    purple
}
