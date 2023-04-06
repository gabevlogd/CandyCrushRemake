using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeCombosState : StateBase<GameState>
{
    private CombosCalculator combosCalculator;

    public ComputeCombosState(CombosCalculator calculator)
    {
        StateID = GameState.ComputeCombos;
        combosCalculator = calculator;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        combosCalculator.gameObject.SetActive(true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        combosCalculator.gameObject.SetActive(false);
    }
}
