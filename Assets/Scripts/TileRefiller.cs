using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRefiller : MonoBehaviour
{
    public Candy CandyPrefab;
    private Tile m_tile;

    private void OnEnable()
    {
        m_tile = GetComponent<Tile>();
    }

    private void OnDisable()
    {
        
    }

    private void StartRefilling()
    {
        int row = m_tile.Data.Row;
        int column = m_tile.Data.Column;

        if (row == GridManager.Instance.MaxRow)
        {
            Candy newCandy = Instantiate(CandyPrefab, transform);
            CandyColor candyColor = (CandyColor)GridManager.RandomColor();
            newCandy.Initialize(0, 0, candyColor, newCandy.CandySprites[(int)candyColor]);
            return;
        }

        Candy candy = GridManager.Instance.Tiles[row + 1, column].GetComponentInChildren<Candy>();

        if (candy != null) candy.transform.SetParent(transform, false);
        
        GridManager.Instance.Tiles[row + 1, column].GetComponent<TileRefiller>().enabled = true;

        this.enabled = false;
    }
}
