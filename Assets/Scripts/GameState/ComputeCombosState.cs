using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeCombosState : StateBase<GameState>
{
    private float m_computationTimer;
    private bool m_finderEnabled;
    private CombosFinder m_combosFinder;

    public ComputeCombosState(CombosFinder finder)
    {
        StateID = GameState.ComputeCombos;
        m_combosFinder = finder;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        m_combosFinder.enabled = false;
        m_computationTimer = 2f;
        m_finderEnabled = false;

        if (GameManager.Instance.PreviousState.StateID == GameState.WaitMove)
        {
            GridManager.PressedTiles[0].GetComponentInChildren<ScoreCalculator>().enabled = true;
            GridManager.PressedTiles[1].GetComponentInChildren<ScoreCalculator>().enabled = true;
        }
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_computationTimer > 0) m_computationTimer -= Time.deltaTime;
        else if (m_finderEnabled == false)
        {
            m_finderEnabled = true;
            m_combosFinder.enabled = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
