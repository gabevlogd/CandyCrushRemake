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
        //Debug.Log("Actived");
        m_gridManager = GridManager.Instance;
        m_startingTile = new Vector2Int(-1, -1);
        m_endingTile = new Vector2Int(-1, -1);

        LooksForHorizontalCombos();
        LooksForVerticalCombos();
        this.enabled = false;
    }

    private void OnDisable()
    {

        NextState();
    }

    /// <summary>
    /// Determines the next phase of the game
    /// </summary>
    private void NextState()
    {
        bool restartCycle = true;
        for (int i = 0; i < GridManager.Instance.MaxColumn; i++)
        {
            if (RefillManager.TilesToRefill[i].Count > 0) restartCycle = false;
        }
        if (restartCycle)
        {
            BugFix();
            GameManager.Instance.ChangeState(GameState.WaitMove);
        }
        else Destroyer.enabled = true;
    }

    /// <summary>
    /// Checks if any horizontal combos have been auto-generated after the last refill of grid
    /// </summary>
    private void LooksForHorizontalCombos()
    {
        for (int i = 0; i < m_gridManager.MaxRow; i++)
        {
            for (int j = 0; j < m_gridManager.MaxColumn; j++)
            {
                Candy currentCandy = GridManager.GetCandy(i, j);


                if (j == 0)
                {
                    StoreFirstTileOfCombo(i, j, currentCandy);
                    continue;
                }

                if (currentCandy != null && currentCandy.Data.candyColor == m_currentColor)
                {
                    m_endingTile = new Vector2Int(j, i);
                    continue;
                }
                else ChecksHorizontalComboValidity(i, j, currentCandy);
            }

            int lastColumn = m_gridManager.MaxColumn - 1;
            Candy lastCandy = GridManager.GetCandy(i, lastColumn);
            ChecksHorizontalComboValidity(i, lastColumn, lastCandy);
        }
    }

    /// <summary>
    /// /// <summary>
    /// Checks if any vertical combos have been auto-generated after the last refill of grid
    /// </summary>
    private void LooksForVerticalCombos()
    {
        for (int i = 0; i < m_gridManager.MaxColumn; i++)
        {
            for (int j = 0; j < m_gridManager.MaxRow; j++)
            {
                Candy currentCandy = GridManager.GetCandy(j, i);

                if (j == 0)
                {
                    StoreFirstTileOfCombo(j, i, currentCandy);
                    continue;
                }

                if (currentCandy != null && currentCandy.Data.candyColor == m_currentColor)
                {
                    m_endingTile = new Vector2Int(i, j);
                    continue;
                }
                else ChecksVerticalComboValidity(j, i, currentCandy);
            }

            int lastRow = m_gridManager.MaxRow - 1;
            Candy lastCandy = GridManager.GetCandy(lastRow, i);
            ChecksVerticalComboValidity(lastRow, i, lastCandy);
        }
    }

    private void StoreFirstTileOfCombo(int row, int column, Candy candy)
    {
        if (candy != null) m_currentColor = candy.Data.candyColor;

        m_startingTile = new Vector2Int(column, row);
        m_endingTile = new Vector2Int(-1, -1);
    }


    private void ChecksHorizontalComboValidity(int row, int column, Candy candy)
    {
        if (m_startingTile.x == -1 || m_endingTile.x == -1)
        {
            StoreFirstTileOfCombo(row, column, candy);
            return;
        }

        Vector2Int combo = m_endingTile - m_startingTile;

        if (combo.magnitude >= 2)
        {
            for (int k = m_startingTile.x; k <= m_endingTile.x; k++)
            {
                Tile curTile = GridManager.GetTile(m_startingTile.y, k);
                if (curTile == null) continue;

                Candy curCandy = GridManager.GetCandy(curTile);


                if (curCandy != null && curCandy.Data.AlreadyAdded == false)
                {
                    curCandy.Data.AlreadyAdded = true;
                    RefillManager.TilesToRefill[k].Add(curTile);
                    AddToDestroyer(curCandy);
                }
            }
        }

        StoreFirstTileOfCombo(row, column, candy);
    }

    private void ChecksVerticalComboValidity(int row, int column, Candy candy)
    {
        if (m_startingTile.x == -1 || m_endingTile.x == -1)
        {
            StoreFirstTileOfCombo(row, column, candy);
            return;
        }

        Vector2Int combo = m_endingTile - m_startingTile;

        if (combo.magnitude >= 2)
        {
            for (int k = m_startingTile.y; k <= m_endingTile.y; k++)
            {
                Tile curTile = GridManager.GetTile(k, m_startingTile.x);
                if (curTile == null) continue;

                Candy curCandy = GridManager.GetCandy(curTile);


                if (curCandy != null && curCandy.Data.AlreadyAdded == false)
                {
                    curCandy.Data.AlreadyAdded = true;
                    RefillManager.TilesToRefill[m_startingTile.x].Add(curTile);
                    AddToDestroyer(curCandy);
                }
            }
        }

        StoreFirstTileOfCombo(row, column, candy);
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


    /// <summary>
    /// Sorry for this
    /// </summary>
    private void BugFix()
    {
        foreach(Tile tile in m_gridManager.Tiles)
        {
            if (tile.transform.childCount == 1) continue;

            if (tile.transform.childCount == 0)
            {
                GridManager.SpawnNewCandy(tile.transform);
                continue;
            }

            if (tile.transform.childCount > 1)
            {
                for (int i = 1; i < tile.transform.childCount; i++)
                {
                    Destroy(tile.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    
}
