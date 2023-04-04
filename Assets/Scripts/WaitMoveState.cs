using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitMoveState : StateBase<GameState>
{

    public WaitMoveState()
    {
        StateID = GameState.WaitMove;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (GridManager.PressedTiles[1])
        {
            if (LegalMove())
            {
                Candy candy0 = GridManager.PressedTiles[0].gameObject.GetComponentInChildren<Candy>();
                Candy candy1 = GridManager.PressedTiles[1].gameObject.GetComponentInChildren<Candy>();

                SwapCandies(candy0, candy1);

                WaitForNewMove();
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    /// <summary>
    /// Returns false if the player attempts to move a candy to an illegal position
    /// </summary>
    /// <returns></returns>
    private bool LegalMove()
    {
        //Debug.Log("LegalTile");
        Tile pressedTile = GridManager.PressedTiles[1];
        Tile lastPressedTile = GridManager.PressedTiles[0];

        Vector2 pressedVec = new Vector2(pressedTile.Data.Column, pressedTile.Data.Row);
        Vector2 lastPressedVec = new Vector2(lastPressedTile.Data.Column, lastPressedTile.Data.Row);
        Vector2 vecBetween = lastPressedVec - pressedVec;

        if (Mathf.Abs(Vector2.Dot(vecBetween, Vector2.up)) == 1 || Mathf.Abs(Vector2.Dot(vecBetween, Vector2.right)) == 1)
        {
            if (pressedVec.x == lastPressedVec.x || pressedVec.y == lastPressedVec.y) return true;
        }

        WaitForNewMove();
        return false;
    }

    private void WaitForNewMove()
    {
        //stops visual feedbacks of selection
        GridManager.PressedTiles[0].GetComponentInChildren<Candy>().Animator.SetBool("Selected", false); 
        GridManager.PressedTiles[1].GetComponentInChildren<Candy>().Animator.SetBool("Selected", false);

        GridManager.PressedTiles[0] = null;
        GridManager.PressedTiles[1] = null;
    }

    private void SwapCandies(Candy candy0, Candy candy1)
    {
        candy0.transform.SetParent(GridManager.PressedTiles[1].transform, false);
        candy1.transform.SetParent(GridManager.PressedTiles[0].transform, false);
    }
}
