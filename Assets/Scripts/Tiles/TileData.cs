using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public int Row;
    public int Column;

    public TileData(int newRow, int newColumn)
    {
        Row = newRow;
        Column = newColumn;
    }
}
