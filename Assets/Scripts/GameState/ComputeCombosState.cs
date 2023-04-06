using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeCombosState : StateBase<GameState>
{

    public ComputeCombosState()
    {
        StateID = GameState.ComputeCombos;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GridManager.PressedTiles[0].GetComponentInChildren<ScoreCalculator>().enabled = true;
        GridManager.PressedTiles[1].GetComponentInChildren<ScoreCalculator>().enabled = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
