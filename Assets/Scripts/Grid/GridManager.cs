using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    public Tile TilePrefab;
    public Candy CandyPrefab;
    public int MaxRow;
    public int MaxColumn;
    public Dictionary<int[], Tile> MapTiles = new Dictionary<int[], Tile>();

    private GridLayoutGroup m_gridData;
    private int m_lastColor, m_secondLastColor;

    public static Tile[] PressedTiles;

    private void Awake()
    {
        PressedTiles = new Tile[2];
        m_lastColor = -1;
        m_secondLastColor = -1;
        InitializeGridData();
        
    }

    private void Start()
    {
        GenerateGrid();
    }



    /// <summary>
    /// Generates the grid and fills it whit candys
    /// </summary>
    private void GenerateGrid()
    {
        for (int row = 0; row < MaxRow; row++)
        {
            for (int column = 0; column < MaxColumn; column++)
            {
                Tile tile = Instantiate(TilePrefab, transform);
                Candy candy = Instantiate(CandyPrefab, tile.transform);
                CandyColor candyColor = CheckColor(RandomColor(), row, column);

                tile.Initialize(this, row, column);
                candy.Initialize(0, 0, candyColor, candy.CandySprites[(int)candyColor]);

                tile.name = "Tile - (" + row.ToString() + " - " + column.ToString() + ")";
                int[] newKeyMap = { row, column };
                //MapTiles[newKeyMap] = tile;
                MapTiles.Add(newKeyMap, tile);
            }
        }
    }

    private void InitializeGridData()
    {
        m_gridData = GetComponent<GridLayoutGroup>();
        m_gridData.startCorner = GridLayoutGroup.Corner.LowerLeft;
        m_gridData.startAxis = GridLayoutGroup.Axis.Horizontal;
        m_gridData.childAlignment = TextAnchor.MiddleCenter;
        m_gridData.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        m_gridData.constraintCount = MaxColumn;
    }

    /// <summary>
    /// Prevents three tiles of the same color from spawning one after the other horizontally
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Return randomly 1 or -1;
    /// </summary>
    /// <returns></returns>
    public static int RandomSign()
    {
        if (Random.value > 0.5) return 1;
        else return -1;
    }

    private CandyColor CheckColor(int color, int y, int x)
    {
        bool checkVertical = false;
        bool checkHorizontal = false;

        int[] verticalKeyMapOne = { y-1, x };
        int[] verticalKeyMapTwo = { y-2, x };
        int[] horizontalKeyMapOne = { y, x-1 };
        int[] horizontalKeyMapTwo = { y, x-2 };

        Candy onRightSideOne = null;
        Candy onRightSideTwo = null;
        Candy onBelowOne = null;
        Candy onBelowTwo = null;

        if (MapTiles.TryGetValue(verticalKeyMapTwo, out Tile tile1) == true)
        {
            onBelowOne = MapTiles[verticalKeyMapOne].GetComponentInChildren<Candy>();
            onBelowTwo = MapTiles[verticalKeyMapTwo].GetComponentInChildren<Candy>();
            checkVertical = true;
        }
        if (MapTiles.TryGetValue(horizontalKeyMapTwo, out Tile tile2) == true)
        {
            onRightSideOne = MapTiles[horizontalKeyMapOne].GetComponentInChildren<Candy>();
            onRightSideTwo = MapTiles[horizontalKeyMapTwo].GetComponentInChildren<Candy>();
            checkHorizontal = true;
        }


        int notAvailableColor = -1;

        if (checkHorizontal && onRightSideOne.Data.candyColor == onRightSideTwo.Data.candyColor)
        {
            notAvailableColor = (int)onRightSideOne.Data.candyColor;
            while (color == notAvailableColor) color = RandomColor();
        }

        if (checkVertical && onBelowOne.Data.candyColor == onBelowTwo.Data.candyColor)
        {
            while (color == (int)onBelowOne.Data.candyColor || color == notAvailableColor) color = RandomColor();
        }

        return (CandyColor)color;
    }

}




