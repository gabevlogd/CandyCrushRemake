using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerScoresData
{
    public int Moves, Points, MathcRating;

    public PlayerScoresData(int moves)
    {
        Moves = moves;
        Points = 0;
        MathcRating = 0;
    }
}
