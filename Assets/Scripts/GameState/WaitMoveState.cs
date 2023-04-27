using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitMoveState : StateBase<GameState>
{

    private GridManager m_gridManager;

    public WaitMoveState(GridManager gridManager)
    {
        StateID = GameState.WaitMove;
        m_gridManager = gridManager;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        WaitForNewMove();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //if PressedTiles[1] != null means that player has chosen both candies to swap
        if (m_gridManager.PressedTiles[1]) CheckMove();
    }

    /// <summary>
    /// if the move is valid it goes to the next state otherwise it waits for a new move
    /// </summary>
    private void CheckMove()
    {
        if (LegalMove())
        {
            Candy candy0 = GridManager.GetCandy(m_gridManager.PressedTiles[0]);
            Candy candy1 = GridManager.GetCandy(m_gridManager.PressedTiles[1]);

            GridManager.SwapCandies(candy0, candy1);
            GameManager.Instance.ChangeState(GameState.ComputeCombos); //go to the next game state
        }
        else WaitForNewMove();
    }

    /// <summary>
    /// Returns false if the player attempts to move a candy to an illegal position
    /// </summary>
    /// <returns></returns>
    private bool LegalMove()
    {
        //Debug.Log("LegalTile");
        Tile pressedTile = m_gridManager.PressedTiles[1];
        Tile lastPressedTile = m_gridManager.PressedTiles[0];

        Vector2Int pressedVec = new Vector2Int(pressedTile.Data.Column, pressedTile.Data.Row);
        Vector2Int lastPressedVec = new Vector2Int(lastPressedTile.Data.Column, lastPressedTile.Data.Row);
        Vector2Int vecBetween = lastPressedVec - pressedVec;

        if (vecBetween.magnitude == 1)
        {
            //stops visual feedbacks of selection
            GridManager.GetCandy(lastPressedTile).Animator.SetBool("Selected", false);
            return true;
        }

        //stops visual feedbacks of selection
        GridManager.GetCandy(lastPressedTile).Animator.SetBool("Selected", false);
        return false;
    }

    private void WaitForNewMove()
    {
        m_gridManager.PressedTiles[0] = null;
        m_gridManager.PressedTiles[1] = null;
    }


}


//if (Mathf.Abs(Vector2.Dot(vecBetween, Vector2.up)) == 1 || Mathf.Abs(Vector2.Dot(vecBetween, Vector2.right)) == 1)
//{
//    if (pressedVec.x == lastPressedVec.x || pressedVec.y == lastPressedVec.y)
//    {
//      //stops visual feedbacks of selection
//        lastPressedTile.GetComponentInChildren<Candy>().Animator.SetBool("Selected", false);
//        return true;
//    }
//}
