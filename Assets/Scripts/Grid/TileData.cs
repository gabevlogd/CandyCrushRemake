using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public uint row;
    public uint column;
    public GridManager gm;

    public TileData(GridManager gridManager, uint newRow, uint newColumn)
    {
        row = newRow;
        column = newColumn;
        gm = gridManager;
    }
}
