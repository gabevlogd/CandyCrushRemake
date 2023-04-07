using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    private Candy m_candy;
    private List<Candy> m_horizontalTwin;
    private List<Candy> m_verticalTwin;

    private void OnEnable()
    {
        m_horizontalTwin = new List<Candy>();
        m_verticalTwin = new List<Candy>();
        m_candy = GetComponent<Candy>();

        CalculateScore();
    }

    private void OnDisable()
    {
        CheckScore();
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
                    CombosFinder.RedCandiesDestroyer += DestoryCandy;
                    break;
                case CandyColor.blue:
                    CombosFinder.BlueCandiesDestroyer += DestoryCandy;
                    break;
                case CandyColor.green:
                    CombosFinder.GreenCandiesDestroyer += DestoryCandy;
                    break;
                case CandyColor.yellow:
                    CombosFinder.YellowCandiesDestroyer += DestoryCandy;
                    break;
            }
        }
    }

    /// <summary>
    /// memorizes the tiles that will remain empty after the destruction of the combos
    /// </summary>
    private void UpdateRefillData()
    {
        int column = m_candy.GetComponentInParent<Tile>().Data.Column;

        RefillManager.TilesToRefill[column].Add(m_candy.GetComponentInParent<Tile>());

        if (m_verticalTwin.Count == 2)
        {
            if (m_verticalTwin[0].Data.Vscore == 1) RefillManager.TilesToRefill[column].Add(m_verticalTwin[0].GetComponentInParent<Tile>());
            if (m_verticalTwin[1].Data.Vscore == 1) RefillManager.TilesToRefill[column].Add(m_verticalTwin[1].GetComponentInParent<Tile>());
        }
        else
        {
            int column0 = m_horizontalTwin[0].GetComponentInParent<Tile>().Data.Column;
            int column1 = m_horizontalTwin[1].GetComponentInParent<Tile>().Data.Column;

            if (m_horizontalTwin[0].Data.Hscore == 1) RefillManager.TilesToRefill[column0].Add(m_horizontalTwin[0].GetComponentInParent<Tile>());
            if (m_horizontalTwin[1].Data.Hscore == 1) RefillManager.TilesToRefill[column1].Add(m_horizontalTwin[1].GetComponentInParent<Tile>());
        }
    }

    /// <summary>
    /// Calculates vertical and horizontal score of the current candy
    /// </summary>
    private void CalculateScore()
    {
        int row = m_candy.GetComponentInParent<Tile>().Data.Row;
        int column = m_candy.GetComponentInParent<Tile>().Data.Column;
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
        if (row == GridManager.Instance.MaxRow - 1) return;

        Candy neighbour = GridManager.Instance.Tiles[row + 1, column].GetComponentInChildren<Candy>();

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

        Candy neighbour = GridManager.Instance.Tiles[row - 1, column].GetComponentInChildren<Candy>();

        if (neighbour != null) UpdateScores(neighbour, true);
    }

    /// <summary>
    /// if the candy on the right has the same color of the current one increment the horizontal score
    /// </summary>
    /// <param name="row">row of the current candy</param>
    /// <param name="column">column of the current candy</param>
    private void CheckNeighbourRight(int row, int column)
    {
        if (column == GridManager.Instance.MaxColumn - 1) return;

        Candy neighbour = GridManager.Instance.Tiles[row, column + 1].GetComponentInChildren<Candy>();

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

        Candy neighbour = GridManager.Instance.Tiles[row, column - 1].GetComponentInChildren<Candy>();

        if (neighbour != null) UpdateScores(neighbour, false);
    }

    /// <summary>
    /// Update the vertical or horizontal score of the current candy
    /// </summary>
    /// <param name="neighbour"></param>
    /// <param name="vertical">if true updates vertical score else horizontal</param>
    private void UpdateScores(Candy neighbour, bool vertical)
    {
        if (neighbour.Data.candyColor == m_candy.Data.candyColor /*&& m_candy.Data.TriggeredBy != neighbour*/)
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
            //neighbour.Data.TriggeredBy = m_candy; //DEVO ANCORA CAPIRE SE SERVE QUESTO CONTROLLO
            neighbour.GetComponent<ScoreCalculator>().enabled = true; //enable the score calculator of the neighbour (chain reaction)
        }
    }

    /// <summary>
    /// Destroys the current candy and its twins
    /// </summary>
    public void DestoryCandy()
    {
        if (m_candy.Data.Hscore >= 2)
        {
            foreach(Candy candy in m_horizontalTwin)
            {
                if (candy.Data.Hscore == 1) Destroy(candy.gameObject);
            }
        }
        else
        {
            foreach (Candy candy in m_verticalTwin)
            {
                if (candy.Data.Vscore == 1) Destroy(candy.gameObject);
            }
        }

        Destroy(gameObject);
    }
}
