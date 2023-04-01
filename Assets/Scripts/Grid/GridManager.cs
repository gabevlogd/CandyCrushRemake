using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Tile tilePrefab;
    public uint maxRow;
    public uint maxColumn;
    private Grid gridData;
    public Dictionary<uint[], TileData> mapTiles = new Dictionary<uint[], TileData>();

    private void Awake()
    {
        gridData = GetComponent<Grid>();
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        Vector3 startPosition = new Vector3(maxColumn * (gridData.cellSize.x + gridData.cellGap.x) / 2, 0, maxRow * (gridData.cellSize.y + gridData.cellGap.y) / 2);
        float x = startPosition.x;
        float y = startPosition.y;

        for (uint row = maxRow; row > 0; row--)
        {
            for(uint column = 0; column < maxColumn; column++)
            {
                var tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                tile.transform.localScale = gridData.cellSize;
                x -= 1 * (gridData.cellSize.x + gridData.cellGap.x);
                tile.Initialize(this, row, column);
                tile.name = "Tile - (" + row.ToString() + " - " + column.ToString() + ")";
                uint[] newKeyMap = { row, column };
                mapTiles[newKeyMap] = tile.data;
            }
            x = startPosition.x;
            y -= 1 * (gridData.cellSize.y + gridData.cellGap.y);
        }
    }
}
