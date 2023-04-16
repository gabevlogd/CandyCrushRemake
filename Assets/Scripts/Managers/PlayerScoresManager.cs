using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoresManager : MonoBehaviour
{
    public int AvailablesMoves, TargetPoints, PointsPerCandy;

    public static bool PointsUpdateNeeded;
    public static bool MovesUpdateNeeded;

    public PlayerScoresData Data;

    private HUDManager m_HUDManager;

    

    private void Awake()
    {
        m_HUDManager = GameManager.Instance.HUDManager;
        Data = new PlayerScoresData(AvailablesMoves);

        m_HUDManager.UpdatePointsUI(Data.Points);
        m_HUDManager.UpdateMovesUI(Data.Moves);
    }

    private void Update()
    {
        if (PointsUpdateNeeded) UpdatePoints();
        if (MovesUpdateNeeded) UpdateMoves();
    }

    /// <summary>
    /// Updates the points of the game
    /// </summary>
    private void UpdatePoints()
    {
        for(int i = 0; i < GridManager.Instance.MaxColumn; i++)
        {
            foreach(Tile tile in RefillManager.TilesToRefill[i])
            {
                Data.Points += PointsPerCandy;
            }
        }

        m_HUDManager.UpdatePointsUI(Data.Points);
        PointsUpdateNeeded = false;
    }

    /// <summary>
    /// Updates the number of remaining moves
    /// </summary>
    private void UpdateMoves()
    {
        Data.Moves--;
        m_HUDManager.UpdateMovesUI(Data.Moves);
        MovesUpdateNeeded = false;
    }
}
