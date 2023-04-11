using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostRefillState : StateBase<GameState>
{
    private PostRefillCombosManager m_postRefillCombosManager;
    private float m_computationTimer;
    private bool m_postRefillManagerEnabled;

    public PostRefillState(PostRefillCombosManager postRefillCombos)
    {
        StateID = GameState.PostRefill;
        m_postRefillCombosManager = postRefillCombos;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        m_computationTimer = 2f;
        m_postRefillManagerEnabled = false;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (m_computationTimer > 0) m_computationTimer -= Time.deltaTime;
        else if (m_postRefillManagerEnabled == false)
        {
            m_postRefillManagerEnabled = true;
            m_postRefillCombosManager.enabled = true;
        }
    }

}
