using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    private Candy m_candy;
    private GridManager m_gridManager;
    private RefillManager m_refillManager;
    private List<Candy> m_horizontalTwin;
    private List<Candy> m_verticalTwin;

    private void OnEnable()
    {
        m_horizontalTwin = new List<Candy>();
        m_verticalTwin = new List<Candy>();
        m_candy = GetComponent<Candy>();
        m_gridManager = GameManager.Instance.GridManager;
        m_refillManager = GameManager.Instance.RefillManager;

        CalculateScore();
    }

    private void OnDisable()
    {
        CheckScore();
        m_candy.Data.TriggeredBy = null;
    }

    /// <summary>
    /// checks if the candy forms a combo and if so, adds it to the combo destroyers
    /// </summary>
    private void CheckScore()
    {
        if (m_candy.Data.Vscore >= 2 || m_candy.Data.Hscore >= 2)
        {
            UpdateRefillData();

            switch (m_candy.Data.candyColor)
            {
                case CandyColor.red:
                    Destroyer.RedCandiesDestroyer += DestroyCandy;
                    break;
                case CandyColor.blue:
                    Destroyer.BlueCandiesDestroyer += DestroyCandy;
                    break;
                case CandyColor.green:
                    Destroyer.GreenCandiesDestroyer += DestroyCandy;
                    break;
                case CandyColor.yellow:
                    Destroyer.YellowCandiesDestroyer += DestroyCandy;
                    break;
                case CandyColor.purple:
                    Destroyer.PurpleCandiesDestroyer += DestroyCandy;
                    break;
            }
        }
    }

    /// <summary>
    /// memorizes the tiles that will remain empty after the destruction of the combos
    /// </summary>
    private void UpdateRefillData()
    {
        int column = GridManager.GetTile(m_candy).Data.Column;

        if (!m_candy.Data.AlreadyAdded)
        {
            m_candy.Data.AlreadyAdded = true;
            m_refillManager.TilesToRefill[column].Add(GridManager.GetTile(m_candy)); 
        }

        if (m_candy.Data.Vscore == 2)
        {
            if (!m_verticalTwin[0].Data.AlreadyAdded && m_verticalTwin[0].Data.Vscore == 1)
            {
                m_verticalTwin[0].Data.AlreadyAdded = true;
                m_refillManager.TilesToRefill[column].Add(GridManager.GetTile(m_verticalTwin[0]));
            }

            if (!m_verticalTwin[1].Data.AlreadyAdded && m_verticalTwin[1].Data.Vscore == 1)
            {
                m_verticalTwin[1].Data.AlreadyAdded = true;
                m_refillManager.TilesToRefill[column].Add(GridManager.GetTile(m_verticalTwin[1]));
            }

            return;
        }

        if (m_candy.Data.Hscore == 2)
        {
            int column0 = GridManager.GetTile(m_horizontalTwin[0]).Data.Column;
            int column1 = GridManager.GetTile(m_horizontalTwin[1]).Data.Column;

            if (!m_horizontalTwin[0].Data.AlreadyAdded && m_horizontalTwin[0].Data.Hscore == 1)
            {
                m_horizontalTwin[0].Data.AlreadyAdded = true;
                m_refillManager.TilesToRefill[column0].Add(GridManager.GetTile(m_horizontalTwin[0]));
            }
            if (!m_horizontalTwin[1].Data.AlreadyAdded && m_horizontalTwin[1].Data.Hscore == 1)
            {
                m_horizontalTwin[1].Data.AlreadyAdded = true;
                m_refillManager.TilesToRefill[column1].Add(GridManager.GetTile(m_horizontalTwin[1]));
            }

            return;
        }
    }

    /// <summary>
    /// Calculates vertical and horizontal score of the current candy
    /// </summary>
    private void CalculateScore()
    {
        int row = GridManager.GetTile(m_candy).Data.Row;
        int column = GridManager.GetTile(m_candy).Data.Column;
        m_candy.Data.Vscore = 0;
        m_candy.Data.Hscore = 0;

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    CheckNeighbourUp(row, column);
                    break;
                case 1:
                    CheckNeighbourDown(row, column);
                    break;
                case 2:
                    CheckNeighbourRight(row, column);
                    break;
                case 3:
                    CheckNeighbourLeft(row, column);
                    break;
            }
        }

        this.enabled = false;
    }

    /// <summary>
    /// if the candy above has the same color of the current one increment the vertical score
    /// </summary>
    /// <param name="row">row of the current candy</param>
    /// <param name="column">column of the current candy</param>
    private void CheckNeighbourUp(int row, int column)
    {
        if (row == m_gridManager.MaxRow - 1) return;

        Candy neighbour = GridManager.GetCandy(m_gridManager.Tiles[row + 1, column]);

        if (neighbour != null) UpdateScores(neighbour, true);
    }

    /// <summary>
    /// if the candy below has the same color of the current one increment the vertical score
    /// </summary>
    /// <param name="row">row of the current candy</param>
    /// <param name="column">column of the current candy</param>
    private void CheckNeighbourDown(int row, int column)
    {
        if (row == 0) return;

        Candy neighbour = GridManager.GetCandy(m_gridManager.Tiles[row - 1, column]);

        if (neighbour != null) UpdateScores(neighbour, true);
    }

    /// <summary>
    /// if the candy on the right has the same color of the current one increment the horizontal score
    /// </summary>
    /// <param name="row">row of the current candy</param>
    /// <param name="column">column of the current candy</param>
    private void CheckNeighbourRight(int row, int column)
    {
        if (column == m_gridManager.MaxColumn - 1) return;

        Candy neighbour = GridManager.GetCandy(m_gridManager.Tiles[row, column + 1]);

        if (neighbour != null) UpdateScores(neighbour, false);
    }

    /// <summary>
    /// if the candy on the left has the same color of the current one increment the horizontal score
    /// </summary>
    /// <param name="row">row of the current candy</param>
    /// <param name="column">column of the current candy</param>
    private void CheckNeighbourLeft(int row, int column)
    {
        if (column == 0) return;

        Candy neighbour = GridManager.GetCandy(m_gridManager.Tiles[row, column - 1]);

        if (neighbour != null) UpdateScores(neighbour, false);
    }

    /// <summary>
    /// Update the vertical or horizontal score of the current candy
    /// </summary>
    /// <param name="neighbour"></param>
    /// <param name="vertical">if true updates vertical score else horizontal</param>
    private void UpdateScores(Candy neighbour, bool vertical)
    {
        if (neighbour.Data.candyColor == m_candy.Data.candyColor)
        {
            if (vertical)
            {
                m_candy.Data.Vscore++;
                m_verticalTwin.Add(neighbour); 
            }
            else
            {
                m_candy.Data.Hscore++;
                m_horizontalTwin.Add(neighbour);
            }

            //DEVO ANCORA CAPIRE SE SERVE QUESTO CONTROLLO
            if (m_candy.Data.TriggeredBy != neighbour)
            {
                neighbour.Data.TriggeredBy = m_candy;
                neighbour.GetComponent<ScoreCalculator>().enabled = true; //enable the score calculator of the neighbour (chain reaction)
            }
        }
    }

    /// <summary>
    /// Destroys the current candy and its twins
    /// </summary>
    public void DestroyCandy()
    {
        if (m_candy.Data.Hscore >= 2)
        {
            foreach(Candy candy in m_horizontalTwin)
            {
                if (candy.Data.Hscore == 1 && candy != null) Destroy(candy.gameObject);
            }
        }
        else if (m_candy.Data.Vscore >= 2)
        {
            foreach (Candy candy in m_verticalTwin)
            {
                if (candy.Data.Vscore == 1 && candy != null) Destroy(candy.gameObject);
            }
        }

        if (m_candy != null) Destroy(m_candy.gameObject);
    }
}
