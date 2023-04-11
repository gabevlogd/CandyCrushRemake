using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostRefillCombosManager : MonoBehaviour
{
    public Destroyer Destroyer;
     
    private GridManager m_gridManager;
    private CandyColor m_currentColor;
    private Vector2Int m_startingTile;
    private Vector2Int m_endingTile;

    private void OnEnable()
    {
        Debug.Log("Actived");

        m_gridManager = GridManager.Instance;
        m_startingTile = new Vector2Int(-1, -1);
        m_endingTile = new Vector2Int(-1, -1);

        CheckHorizontalCombo();
        this.enabled = false;
    }

    private void OnDisable()
    {

        bool restartCycle = true;
        for (int i = 0; i < GridManager.Instance.MaxColumn; i++)
        {
            if (RefillManager.TilesToRefill[i].Count > 0) restartCycle = false;
        }
        if (restartCycle) GameManager.Instance.ChangeState(GameState.WaitMove);
        else Destroyer.enabled = true;
    }

    private void CheckHorizontalCombo()
    {
        for (int i = 0; i < m_gridManager.MaxRow; i++)
        {
            for (int j = 0; j < m_gridManager.MaxColumn; j++)
            {
                Candy currentCandy = m_gridManager.Tiles[i, j].GetComponentInChildren<Candy>();

                if (j == 0)
                {
                    SetFirstIteration(i, j, currentCandy);
                    continue;
                }

                if (currentCandy.Data.candyColor == m_currentColor)
                {
                    m_endingTile = new Vector2Int(j, i);
                    continue;
                }
                else LastIteration(i, j, currentCandy);
            }

            int lastColumn = m_gridManager.MaxColumn - 1;
            Candy lastCandy = m_gridManager.Tiles[i, lastColumn].GetComponentInChildren<Candy>();
            LastIteration(i, lastColumn, lastCandy);
        }

        Debug.Log("CheckHorizontalCombo ended");
        
    }

    private void SetFirstIteration(int row, int column, Candy candy)
    {
        if (candy != null) m_currentColor = candy.Data.candyColor;

        m_startingTile = new Vector2Int(column, row);
        m_endingTile = new Vector2Int(-1, -1);
    }


    private void LastIteration(int row, int column, Candy candy)
    {
        if (m_startingTile.x == -1 || m_endingTile.x == -1)
        {
            SetFirstIteration(row, column, candy);
            return;
        }

        Vector2Int combo = m_endingTile - m_startingTile;

        if (combo.magnitude >= 2)
        {
            for (int k = m_startingTile.x; k <= m_endingTile.x; k++)
            {
                Tile curTile = m_gridManager.Tiles[combo.y, k];
                Candy curCandy = curTile.GetComponentInChildren<Candy>();

                if (curCandy.Data.AlreadyAdded == false)
                {
                    curCandy.Data.AlreadyAdded = true;
                    RefillManager.TilesToRefill[k].Add(curTile);
                    AddToDestroyer(curCandy);
                }
            }
        }

        SetFirstIteration(row, column, candy);
    }

    private void AddToDestroyer(Candy candy)
    {
        switch (candy.Data.candyColor)
        {
            case CandyColor.red:
                Destroyer.RedCandiesDestroyer += candy.SelfDestruction;
                break;
            case CandyColor.blue:
                Destroyer.BlueCandiesDestroyer += candy.SelfDestruction;
                break;
            case CandyColor.green:
                Destroyer.GreenCandiesDestroyer += candy.SelfDestruction;
                break;
            case CandyColor.yellow:
                Destroyer.YellowCandiesDestroyer += candy.SelfDestruction;
                break;
            case CandyColor.purple:
                Destroyer.PurpleCandiesDestroyer += candy.SelfDestruction;
                break;
        }
    }

    
}
