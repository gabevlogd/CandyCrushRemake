using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillGridState : StateBase<GameState>
{
    private RefillManager m_refillManager;

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
        m_refillManager.enabled = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!m_refillManager.enabled) GameManager.Instance.ChangeState(GameState.WaitMove); //ONLY FOR DEBUG, NOT THE CORRECT NEXT STATE
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
