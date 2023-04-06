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
        //add something to some delegate
        if (m_candy.Data.Vscore >= 2 || m_candy.Data.Hscore >= 2)
        {
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

    private void CheckNeighbourUp(int row, int column)
    {
        if (row == GridManager.Instance.MaxRow - 1) return;

        Candy neighbour = GridManager.Instance.Tiles[row + 1, column].GetComponentInChildren<Candy>();

        UpdateScores(neighbour, true);
    }

    private void CheckNeighbourDown(int row, int column)
    {
        if (row == 0) return;

        Candy neighbour = GridManager.Instance.Tiles[row - 1, column].GetComponentInChildren<Candy>();

        UpdateScores(neighbour, true);
    }

    private void CheckNeighbourRight(int row, int column)
    {
        if (column == GridManager.Instance.MaxColumn - 1) return;

        Candy neighbour = GridManager.Instance.Tiles[row, column + 1].GetComponentInChildren<Candy>();

        UpdateScores(neighbour, false);
    }

    private void CheckNeighbourLeft(int row, int column)
    {
        if (column == 0) return;

        Candy neighbour = GridManager.Instance.Tiles[row, column - 1].GetComponentInChildren<Candy>();

        UpdateScores(neighbour, false);
    }

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
            neighbour.GetComponent<ScoreCalculator>().enabled = true;
        }
    }

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
