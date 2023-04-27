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
    public Tile[,] Tiles;
    public Tile[] PressedTiles;

    private static GridManager m_instance;
    private GridLayoutGroup m_gridData;


    private void Awake()
    {
        m_instance = this;

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

                tile.Initialize(row, column);
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
        //where to store the candies if it exists in the positions listed above
        Candy onRightSideOne = GetCandy(y, x - 1);
        Candy onRightSideTwo = GetCandy(y, x - 2);
        Candy onBelowOne = GetCandy(y - 1, x);
        Candy onBelowTwo = GetCandy(y - 2, x);


        int notAvailableColor = -1;

        if (onRightSideTwo != null && onRightSideOne.Data.candyColor == onRightSideTwo.Data.candyColor) //check if the colors of the stored candies are equals to the current candy color to analyze (Horizontally)
        {
            notAvailableColor = (int)onRightSideOne.Data.candyColor;
            while (color == notAvailableColor) color = RandomColor();
        }

        if (onBelowTwo != null && onBelowOne.Data.candyColor == onBelowTwo.Data.candyColor) //check if the colors of the stored candies are equals to the current candy color to analyze (Vertically)
        {
            while (color == (int)onBelowOne.Data.candyColor || color == notAvailableColor) color = RandomColor();
        }

        return (CandyColor)color;
    }



    /////////////////////////////////////////////////////////////////////////////////////////////
    //********************************** STATIC METHODS *****************************************
    /////////////////////////////////////////////////////////////////////////////////////////////

    #region STATIC METHODS

    /// <summary>
    /// </summary>
    /// <returns>random integer between 0 and 3 inclusive</returns>
    public static int RandomColor()
    {
        return Random.Range(0, 5);
    }

    /// <summary>
    /// </summary>
    /// <returns>1 or -1 randomly</returns>
    public static int RandomSign()
    {
        if (Random.value > 0.5) return 1;
        else return -1;
    }

    /// <summary>
    /// Returns the candy in the passed coordinates
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public static Candy GetCandy(int row, int column)
    {
        //checks if passed coordinates are valid
        if (row < 0 || row >= m_instance.MaxRow || column < 0 || column >= m_instance.MaxColumn)
        {
            Debug.Log("NOT VALID COORDINATES");
            return null;
        }

        Tile tile = m_instance.Tiles[row, column];
        return GetCandy(tile);
    }

    /// <summary>
    /// Returns the candy of the passed tile if there are in it
    /// </summary>
    /// <param name="tile">tile where to look for candy</param>
    /// <returns>candy in the tile</returns>
    public static Candy GetCandy(Tile tile)
    {
        if (tile == null)
        {
            Debug.Log("NULL TILE PASSED");
            return null;
        }

        int childCount = tile.transform.childCount;
        Candy candyToReturn = null;

        //checks if there are candies in tile
        if (childCount == 0)
        {
            Debug.Log("CANDY NOT FOUND");
            return candyToReturn;
        }

        //checks if there are more than one candy in tile
        if (childCount > 1)
        {
            //gets the first candy and destroy the others
            for (int i = 0; i < childCount; i++)
            {
                if (i == 0) candyToReturn = tile.transform.GetChild(i).GetComponent<Candy>();
                else Destroy(tile.transform.GetChild(i));
            }

            Debug.Log("MULTIPLE CANDY FOUND");
            return candyToReturn;
        }

        candyToReturn = tile.GetComponentInChildren<Candy>();
        return candyToReturn;
    }

    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns>the tile in the passed coordinates</returns>
    public static Tile GetTile(int row, int column)
    {
        if (row < 0 || row >= m_instance.MaxRow || column < 0 || column >= m_instance.MaxColumn)
        {
            Debug.Log("NOT VALID COORDINATES");
            return null;
        }

        return m_instance.Tiles[row, column];
    }

    /// <summary>
    /// Returns the tile of the passed candy
    /// </summary>
    /// <param name="candy">candy of the wanted tile</param>
    /// <returns>tile of the candy</returns>
    public static Tile GetTile(Candy candy)
    {
        return candy.GetComponentInParent<Tile>();
    }

    /// <summary>
    /// Spawns a candy in the passed parent's position 
    /// </summary>
    /// <param name="parent">The transform of the tile where to spawn the candy</param>
    public static void SpawnNewCandy(Transform parent)
    {
        Candy candy = Instantiate(m_instance.CandyPrefab, parent);
        CandyColor candyColor = (CandyColor)GridManager.RandomColor();
        candy.Initialize(0, 0, candyColor, candy.CandySprites[(int)candyColor]);
    }

    /// <summary>
    /// swaps the position of two candies
    /// </summary>
    /// <param name="candy0"></param>
    /// <param name="candy1"></param>
    public static void SwapCandies(Candy candy0, Candy candy1)
    {
        Transform parent0 = candy0.transform.parent;
        Transform parent1 = candy1.transform.parent;
        candy0.transform.SetParent(parent1, false);
        candy1.transform.SetParent(parent0, false);
    }

    #endregion



}







