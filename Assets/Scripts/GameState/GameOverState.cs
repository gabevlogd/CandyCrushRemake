using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : StateBase<GameState>
{
    private GameObject m_winTab, m_loseTab;

    public GameOverState(GameObject winTab, GameObject loseTab)
    {
        StateID = GameState.GameOver;
        m_winTab = winTab;
        m_loseTab = loseTab;
    }

    public override void OnEnter()
    {
        GameManager.Instance.PlayerScoresManager.enabled = false;
        GameManager.Instance.HUDManager.gameObject.SetActive(false);
        GridManager.Instance.gameObject.SetActive(false);

        if (GameManager.Instance.GameWon) m_winTab.gameObject.SetActive(true);
        else m_loseTab.gameObject.SetActive(true);
    }

}
