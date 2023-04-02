using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public Tile tilePrefab;
    public int maxRow;
    public int maxColumn;
    private GridLayoutGroup gridData;
    public Dictionary<int[], TileData> mapTiles = new Dictionary<int[], TileData>();

    private void Awake()
    {
        InitializeGridData();
        
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int row = 0; row < maxRow; row++)
        {
            for (int column = 0; column < maxColumn; column++)
            {
                Tile tile = Instantiate(tilePrefab, transform);
                tile.Initialize(this, row, column);
                tile.name = "Tile - (" + row.ToString() + " - " + column.ToString() + ")";
                int[] newKeyMap = { row, column };
                mapTiles[newKeyMap] = tile.data;
            }
        }
    }

    private void InitializeGridData()
    {
        gridData = GetComponent<GridLayoutGroup>();
        gridData.startCorner = GridLayoutGroup.Corner.UpperLeft;
        gridData.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridData.childAlignment = TextAnchor.MiddleCenter;
        gridData.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridData.constraintCount = maxColumn;
    }
}



///OLD BALLETTA'S METHOD
//private void GenerateGrid()
//{
//    Vector3 startPosition = new Vector3(maxColumn * (gridData.cellSize.x + gridData.cellGap.x) / 2, maxRow * (gridData.cellSize.y + gridData.cellGap.y) / 2, 0);
//    float x = startPosition.x;
//    float y = startPosition.y;

//    for (uint row = maxRow; row > 0; row--)
//    {
//        for(uint column = 0; column < maxColumn; column++)
//        {
//            var tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
//            tile.transform.localScale = gridData.cellSize;
//            x -= 1 * (gridData.cellSize.x + gridData.cellGap.x);
//            tile.Initialize(this, row, column);
//            tile.name = "Tile - (" + row.ToString() + " - " + column.ToString() + ")";
//            uint[] newKeyMap = { row, column };
//            mapTiles[newKeyMap] = tile.data;
//        }
//        x = startPosition.x;
//        y -= 1 * (gridData.cellSize.y + gridData.cellGap.y);
//    }
//}
