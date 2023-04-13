using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillGridState : StateBase<GameState>
{
    private RefillManager m_refillManager;
    private float m_computationTimer;
    private bool m_refillManagerEnabled;

    public RefillGridState(RefillManager refillManager)
    {
        StateID = GameState.RefillGrid;
        m_refillManager = refillManager;

        RefillManager.TilesToRefill = new List<Tile>[GridManager.Instance.MaxColumn];
        for (int i = 0; i < GridManager.Instance.MaxColumn; i++) RefillManager.TilesToRefill[i] = new List<Tile>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        m_computationTimer = 0.2f;
        m_refillManagerEnabled = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_computationTimer > 0) m_computationTimer -= Time.deltaTime;
        else if (m_refillManagerEnabled == false)
        {
            m_refillManagerEnabled = true;
            m_refillManager.enabled = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
