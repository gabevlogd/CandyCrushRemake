using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillManager : MonoBehaviour
{
    public static List<Tile>[] TilesToRefill;

    private int[] m_lowestRows;

    private void OnEnable()
    {
        //TilesToRefill = new List<Tile>[GridManager.Instance.MaxColumn];
        m_lowestRows = new int[GridManager.Instance.MaxColumn];
        FindLowestTiles();
        StartCandiesFall();
    }

    private void FindLowestTiles()
    {
        for(int i = 0; i < GridManager.Instance.MaxColumn; i++)
        {
            if (TilesToRefill[i].Count == 0) continue;
            else m_lowestRows[i] = TilesToRefill[i][0].Data.Row;

            for (int j = 1; j < TilesToRefill[i].Count; j++)
            {
                if (TilesToRefill[i][j].Data.Row > m_lowestRows[i]) continue;
                else m_lowestRows[i] = TilesToRefill[i][j].Data.Row;
            }
            //Debug.Log(m_lowestRows[i]); its ok
            Debug.Log(TilesToRefill[i].Count + " : " + i); //NOT CORRECT
        }
    }

    private void StartCandiesFall()
    {
        for (int i = 0; i < GridManager.Instance.MaxColumn; i++)
        {
            if (TilesToRefill[i].Count == 0) continue;

            int numberOfEmptyTiles = TilesToRefill[i].Count;
            int firstFullTile = m_lowestRows[i] + numberOfEmptyTiles;

            for (int j = firstFullTile; j < GridManager.Instance.MaxRow; j++)
            {
                Candy candy = GridManager.Instance.Tiles[j, i].GetComponentInChildren<Candy>(); 
                candy.transform.SetParent(GridManager.Instance.Tiles[j - numberOfEmptyTiles, i].transform); //set new parent of candy (before the fall)
                candy.GetComponent<GravityComponent>().enabled = true;
            }

        }
    }

}
