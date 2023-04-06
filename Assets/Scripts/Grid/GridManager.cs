using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public Tile TilePrefab;
    public Candy CandyPrefab;

    public int MaxRow;
    public int MaxColumn;
    public Tile[,] Tiles;
    public static Tile[] PressedTiles;

    private GridLayoutGroup m_gridData;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        PressedTiles = new Tile[2];
        Tiles = new Tile[MaxRow, MaxColumn];
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
                Tiles[row, column] = tile;
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
    /// Prevents three tiles of the same color from spawning one after the other horizontally or vertically
    /// </summary>
    /// <param name="color">color to check</param>
    /// <param name="y">row of the candy</param>
    /// <param name="x">column of the candy</param>
    /// <returns></returns>
    private CandyColor CheckColor(int color, int y, int x)
    {
        bool checkVertical = false; //must check if there are combinations of three candies vertically?
        bool checkHorizontal = false; //must check if there are combinations of three candies horizontally?

        //coordinates of the positions to be checked
        int[] verticalKeyMapOne = { y-1, x };
        int[] verticalKeyMapTwo = { y-2, x };
        int[] horizontalKeyMapOne = { y, x-1 };
        int[] horizontalKeyMapTwo = { y, x-2 };

        //where to store the candies if it exists in the positions listed above
        Candy onRightSideOne = null;
        Candy onRightSideTwo = null;
        Candy onBelowOne = null;
        Candy onBelowTwo = null;

        if (y >= 2 && Tiles[verticalKeyMapTwo[0], verticalKeyMapTwo[1]] != null) //if they exists stores the candies along vertically
        {
            onBelowOne = Tiles[verticalKeyMapOne[0], verticalKeyMapOne[1]].GetComponentInChildren<Candy>();
            onBelowTwo = Tiles[verticalKeyMapTwo[0], verticalKeyMapTwo[1]].GetComponentInChildren<Candy>();
            checkVertical = true;
        }
        if (x >= 2 && Tiles[horizontalKeyMapTwo[0], horizontalKeyMapTwo[1]] != null) //if they exists stores the candies along horizontally
        {
            onRightSideOne = Tiles[horizontalKeyMapOne[0], horizontalKeyMapOne[1]].GetComponentInChildren<Candy>();
            onRightSideTwo = Tiles[horizontalKeyMapTwo[0], horizontalKeyMapTwo[1]].GetComponentInChildren<Candy>();
            checkHorizontal = true;
        }


        int notAvailableColor = -1;

        if (checkHorizontal && onRightSideOne.Data.candyColor == onRightSideTwo.Data.candyColor) //check if the colors of the stored candies are equals to the current candy color to analyze (Horizontally)
        {
            notAvailableColor = (int)onRightSideOne.Data.candyColor;
            while (color == notAvailableColor) color = RandomColor();
        }

        if (checkVertical && onBelowOne.Data.candyColor == onBelowTwo.Data.candyColor) //check if the colors of the stored candies are equals to the current candy color to analyze (Vertically)
        {
            while (color == (int)onBelowOne.Data.candyColor || color == notAvailableColor) color = RandomColor();
        }

        return (CandyColor)color;
    }

    /// <summary>
    /// </summary>
    /// <returns>random integer between 0 and 3 inclusive</returns>
    public static int RandomColor()
    {
        return Random.Range(0, 4);
    }

    /// <summary>
    /// </summary>
    /// <returns>1 or -1 randomly</returns>
    public static int RandomSign()
    {
        if (Random.value > 0.5) return 1;
        else return -1;
    }



}



/// <summary>
/// Prevents three tiles of the same color from spawning one after the other horizontally or vertically
/// </summary>
/// <param name="color"></param>
/// <returns></returns>
//private CandyColor CheckColorAdmissibility(int color)
//{
//    if (m_lastColor == -1)
//    {
//        m_lastColor = color;
//        return (CandyColor)color;
//    }
//    if (m_secondLastColor == -1)
//    {
//        m_secondLastColor = color;
//        return (CandyColor)color;
//    }


//    if (m_lastColor == m_secondLastColor)
//    {
//        while (color == m_lastColor) color = RandomColor();

//        m_secondLastColor = m_lastColor;
//        m_lastColor = color;
//        return (CandyColor)color;
//    }


//    m_secondLastColor = m_lastColor;
//    m_lastColor = color;
//    return (CandyColor)color;
//}
