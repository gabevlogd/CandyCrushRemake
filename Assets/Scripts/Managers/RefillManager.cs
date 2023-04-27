using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillManager : MonoBehaviour
{
    public List<Tile>[] TilesToRefill;

    public Candy CandyPrefab;

    private int[] m_lowestRows;
    private int[] m_firstFullRows;
    private GridManager m_gridManager;

    private void OnEnable()
    {
        m_gridManager = GameManager.Instance.GridManager;
        m_lowestRows = new int[m_gridManager.MaxColumn];
        m_firstFullRows = new int[m_gridManager.MaxColumn];

        FindLowestRows();
        FindFirstFullRows();
        StartCandiesFall();
        RefillEmptyTiles();
    }

    private void OnDisable()
    {
        for (int i = 0; i < m_gridManager.MaxColumn; i++) TilesToRefill[i] = new List<Tile>();

        foreach (Tile tile in m_gridManager.Tiles)
        {
            Candy candy = GridManager.GetCandy(tile);
            if (candy != null) candy.Data.ResetData();
        }

        GameManager.Instance.ChangeState(GameState.PostRefill);

    }


    /// <summary>
    /// For each column finds the lowest empty row and stores it 
    /// </summary>
    private void FindLowestRows()
    {
        for(int i = 0; i < m_gridManager.MaxColumn; i++)
        {
            if (TilesToRefill[i].Count == 0) continue;
            else m_lowestRows[i] = TilesToRefill[i][0].Data.Row;

            for (int j = 1; j < TilesToRefill[i].Count; j++)
            {
                if (TilesToRefill[i][j].Data.Row >= m_lowestRows[i]) continue;
                else m_lowestRows[i] = TilesToRefill[i][j].Data.Row;
            }
            //Debug.Log(m_lowestRows[i]);
        }
    }

    /// <summary>
    /// For each column finds the first full row, after the lowest empty one, and stores it 
    /// </summary>
    private void FindFirstFullRows()
    {
        for (int i = 0; i < m_gridManager.MaxColumn; i++)
        {
            if (TilesToRefill[i].Count == 0) continue;

            int firstFullRow = -1;
            if (m_lowestRows[i] < m_gridManager.MaxRow - 1) firstFullRow = m_lowestRows[i] + 1;

            if (firstFullRow != -1)
            {
                while (firstFullRow < m_gridManager.MaxRow && GridManager.GetTile(firstFullRow, i).transform.childCount == 0) firstFullRow++;
            }
            
            m_firstFullRows[i] = firstFullRow;

            //Debug.Log(m_firstFullRows[i]);
        }
    }

    /// <summary>
    /// For each column makes the candies that have to fall, fall
    /// </summary>
    private void StartCandiesFall()
    {
        for (int i = 0; i < m_gridManager.MaxColumn; i++)
        {
            if (TilesToRefill[i].Count == 0 || m_firstFullRows[i] == -1) continue;

            int lowestRow = m_lowestRows[i];

            for (int j = m_firstFullRows[i]; j < m_gridManager.MaxRow; j++)
            {
                Candy candy = GridManager.GetCandy(j, i);

                if (candy == null) continue;

                Tile newParent = GridManager.GetTile(lowestRow++, i);
                if (newParent != null)
                {
                    candy.transform.SetParent(newParent.transform); //set new parent of candy (before the fall)
                    candy.GetComponent<GravityComponent>().enabled = true;
                }
            }
        }
    }

    /// <summary>
    /// Refills the empty tiles
    /// </summary>
    private void RefillEmptyTiles()
    {
        for (int i = 0; i < m_gridManager.MaxColumn; i++)
        {
            if (TilesToRefill[i].Count == 0) continue;

            int rowToRefill = m_gridManager.MaxRow - TilesToRefill[i].Count;

            for (int j = 0; j < TilesToRefill[i].Count; j++)
            {
                Transform parent = m_gridManager.Tiles[rowToRefill++, i].transform;
                GridManager.SpawnNewCandy(parent);
            }
        }

        this.enabled = false;
    }


}
