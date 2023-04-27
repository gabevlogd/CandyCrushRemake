using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoresManager : MonoBehaviour
{
    public int AvailablesMoves, TargetPoints, PointsPerCandy;

    public static bool PointsUpdateNeeded;
    public static bool MovesUpdateNeeded;
    public static bool NewRecord;

    public static PlayerScoresData Data;

    private HUDManager m_HUDManager;

    

    private void Awake()
    {
        m_HUDManager = GameManager.Instance.HUDManager;
        Data = new PlayerScoresData(AvailablesMoves);

        m_HUDManager.UpdatePointsUI(Data.Points, TargetPoints);
        m_HUDManager.UpdateMovesUI(Data.Moves);
    }

    private void Update()
    {
        if (PointsUpdateNeeded) UpdatePoints();
        if (MovesUpdateNeeded) UpdateMoves();
    }

    private void OnDisable()
    {
        UpdateRecord();
        //Debug.Log(PlayerPrefs.GetInt("Record"));
    }

    /// <summary>
    /// Updates the points of the game
    /// </summary>
    private void UpdatePoints()
    {
        for(int i = 0; i < GameManager.Instance.GridManager.MaxColumn; i++)
        {
            foreach(Tile tile in GameManager.Instance.RefillManager.TilesToRefill[i])
            {
                Data.Points += PointsPerCandy;
            }
        }

        m_HUDManager.UpdatePointsUI(Data.Points, TargetPoints);
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

    private void UpdateRecord()
    {
        if (Data.Moves > 0) Data.Record = (float)(Data.Points * Data.Moves) / 100f;
        else Data.Record = (float)(Data.Points) / 100f;

        if (PlayerPrefs.HasKey("Record"))
        {
            if (PlayerPrefs.GetFloat("Record") < Data.Record)
            {
                NewRecord = true;
                PlayerPrefs.SetFloat("Record", Data.Record);
            }
            else NewRecord = false;
        }
        else
        {
            NewRecord = true;
            PlayerPrefs.SetFloat("Record", Data.Record);
        }
    }
}
