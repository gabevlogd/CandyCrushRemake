using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillManager : MonoBehaviour
{
    public static List<Tile>[] TilesToRefill;
    public static List<Candy> CandiesToCheckAfterRefill;
    public Candy CandyPrefab;

    private int[] m_lowestRows;

    private void OnEnable()
    {
        //TilesToRefill = new List<Tile>[GridManager.Instance.MaxColumn];
        m_lowestRows = new int[GridManager.Instance.MaxColumn];
        FindLowestTiles();
        StartCandiesFall();
        RefillEmptyTiles();
    }

    private void OnDisable()
    {
        //foreach(Candy candy in CandiesToCheckAfterRefill)
        //{
        //    candy.GetComponent<ScoreCalculator>().enabled = true;
        //}
        TilesToRefill = new List<Tile>[GridManager.Instance.MaxColumn];
        for (int i = 0; i < GridManager.Instance.MaxColumn; i++) RefillManager.TilesToRefill[i] = new List<Tile>();
    }


    /// <summary>
    /// For each column finds the lowest empty tile and stores it 
    /// </summary>
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
        }
    }

    /// <summary>
    /// For each column makes the candies that have to fall, fall
    /// </summary>
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

                //CandiesToCheckAfterRefill.Add(candy);
            }

        }
    }

    private void RefillEmptyTiles()
    {
        for (int i = 0; i < GridManager.Instance.MaxColumn; i++)
        {
            if (TilesToRefill[i].Count == 0) continue;

            int rowToRefill = GridManager.Instance.MaxRow - TilesToRefill[i].Count;

            for (int j = 0; j < TilesToRefill[i].Count; j++)
            {
                Transform parent = GridManager.Instance.Tiles[rowToRefill++, i].transform;
                StartCoroutine(SpawnNewCandy(parent));
            }
        }

        this.enabled = false;
    }

    private IEnumerator SpawnNewCandy(Transform parent)
    {
        yield return new WaitForSeconds(1f);
        Candy candy = Instantiate(CandyPrefab, parent);
        CandyColor candyColor = (CandyColor)GridManager.RandomColor();
        candy.Initialize(0, 0, candyColor, candy.CandySprites[(int)candyColor]);

        //CandiesToCheckAfterRefill.Add(candy);
    }

}
