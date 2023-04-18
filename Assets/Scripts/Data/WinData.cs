using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinData : MonoBehaviour
{
    public Text Record, Points;

    private void OnEnable()
    {
        Points.text = "Points: " + PlayerScoresManager.Data.Points;

        if (PlayerScoresManager.NewRecord)
        {
            Record.text = "New Record: " + PlayerScoresManager.Data.Record;
        }
        else Record.text = "Record: " + PlayerScoresManager.Data.Record;
    }
}
