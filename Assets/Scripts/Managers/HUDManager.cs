using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Text Points, Moves;

    public void UpdatePointsUI(int points, int targetPoints) => Points.text = "Points: " + points.ToString() + "/" + targetPoints;

    public void UpdateMovesUI(int moves) => Moves.text = "Moves: " + moves.ToString();
}
