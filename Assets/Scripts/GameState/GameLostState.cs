using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLostState : StateBase<GameState>
{
    public GameLostState()
    {
        StateID = GameState.GameLost;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
