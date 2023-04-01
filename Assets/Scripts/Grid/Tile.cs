using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileData data;

    public void Initialize(GridManager gridM, uint rowInit, uint columnInit)
    {
        data = new TileData(gridM, rowInit, columnInit);
    }
}
