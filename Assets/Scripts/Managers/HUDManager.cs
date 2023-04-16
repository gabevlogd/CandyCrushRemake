using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Text Points, Moves;

    public void UpdatePointsUI(int points) => Points.text = "POINTS: " + points.ToString();

    public void UpdateMovesUI(int moves) => Moves.text = "MOVES: " + moves.ToString();
}
