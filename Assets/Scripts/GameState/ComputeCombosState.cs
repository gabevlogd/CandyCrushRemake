using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeCombosState : StateBase<GameState>
{
    private float m_computationTimer;
    private bool m_destroyerEnabled;
    private Destroyer m_destroyer;

    public ComputeCombosState(Destroyer destroyer)
    {
        StateID = GameState.ComputeCombos;
        m_destroyer = destroyer;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        m_destroyer.enabled = false;
        m_computationTimer = 0.2f;
        m_destroyerEnabled = false;

        if (GameManager.Instance.PreviousState.StateID == GameState.WaitMove)
        {
            GridManager.PressedTiles[0].GetComponentInChildren<ScoreCalculator>().enabled = true;
            GridManager.PressedTiles[1].GetComponentInChildren<ScoreCalculator>().enabled = true;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //NEED TO FIND A BETTER SOLUTION
        if (m_computationTimer > 0) m_computationTimer -= Time.deltaTime;
        else if (SwappedCandiesMakeCombo() == false) GameManager.Instance.ChangeState(GameState.WaitMove);
        else if (m_destroyerEnabled == false)
        {
            m_destroyerEnabled = true;
            m_destroyer.enabled = true;
        }
    }

    /// <summary>
    /// Checks if the swapped candies form combos
    /// </summary>
    /// <returns></returns>
    public bool SwappedCandiesMakeCombo()
    {
        Candy candy0 = GridManager.GetCandy(GridManager.PressedTiles[0]);
        Candy candy1 = GridManager.GetCandy(GridManager.PressedTiles[1]);

        if (candy0.Data.AlreadyAdded || candy1.Data.AlreadyAdded)
        {
            PlayerScoresManager.MovesUpdateNeeded = true;
            return true;
        }

        //reswap candies before, becasue only moves that makes combos are alowed
        GridManager.SwapCandies(candy0, candy1);
        return false;
    }
}
