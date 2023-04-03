using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public Tile TilePrefab;
    public Candy CandyPrefab;
    public int MaxRow;
    public int MaxColumn;
    public Dictionary<int[], TileData> MapTiles = new Dictionary<int[], TileData>();

    private GridLayoutGroup m_gridData;
    private int m_lastColor, m_secondLastColor;

    private void Awake()
    {
        m_lastColor = -1;
        m_secondLastColor = -1;
        InitializeGridData();
        
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int row = 0; row < MaxRow; row++)
        {
            for (int column = 0; column < MaxColumn; column++)
            {
                Tile tile = Instantiate(TilePrefab, transform);
                Candy candy = Instantiate(CandyPrefab, tile.transform);
                CandyColor candyColor = CheckColorAdmissibility(RandomColor());

                tile.Initialize(this, row, column);
                candy.Initialize(0, 0, candyColor, candy.CandySprites[(int)candyColor]);

                tile.name = "Tile - (" + row.ToString() + " - " + column.ToString() + ")";
                int[] newKeyMap = { row, column };
                MapTiles[newKeyMap] = tile.Data;
            }
        }
    }

    private void InitializeGridData()
    {
        m_gridData = GetComponent<GridLayoutGroup>();
        m_gridData.startCorner = GridLayoutGroup.Corner.UpperLeft;
        m_gridData.startAxis = GridLayoutGroup.Axis.Horizontal;
        m_gridData.childAlignment = TextAnchor.MiddleCenter;
        m_gridData.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        m_gridData.constraintCount = MaxColumn;
    }

    private CandyColor CheckColorAdmissibility(int color)
    {
        if (m_lastColor == -1)
        {
            m_lastColor = color;
            return (CandyColor)color;
        }
        if (m_secondLastColor == -1)
        {
            m_secondLastColor = color;
            return (CandyColor)color;
        }


        if (m_lastColor == m_secondLastColor)
        {
            while (color == m_lastColor) color = RandomColor();

            m_secondLastColor = m_lastColor;
            m_lastColor = color;
            return (CandyColor)color;
        }


        m_secondLastColor = m_lastColor;
        m_lastColor = color;
        return (CandyColor)color;
    }


    public static int RandomColor()
    {
        return Random.Range(0, 4);
    }

    public static int RandomSign()
    {
        if (Random.value > 0.5) return 1;
        else return -1;
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
