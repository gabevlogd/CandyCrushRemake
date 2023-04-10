using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombosFinder : MonoBehaviour
{
    public delegate void CandiesDestroyer();

    public static CandiesDestroyer RedCandiesDestroyer;
    public static CandiesDestroyer BlueCandiesDestroyer;
    public static CandiesDestroyer GreenCandiesDestroyer;
    public static CandiesDestroyer YellowCandiesDestroyer;
    public static CandiesDestroyer PurpleCandiesDestroyer;


    private void OnEnable()
    {
        //Debug.Log("CombosFinder enabled");
        DestroyCombos();
        this.enabled = false;
    }

    private void OnDisable()
    {
        ResetDestroyer();

        //ONLY FOR DEBUG, REAL VALUE IS GAMESTATE.REFILLGRID
        /////////////////////////////////////////////////////////////////////////////////////////////
        GameState nextGameState = GameState.WaitMove;
        for (int i = 0; i < GridManager.Instance.MaxColumn; i++)
        {
            if (RefillManager.TilesToRefill[i].Count > 0) nextGameState = GameState.RefillGrid;
        }
        GameManager.Instance.ChangeState(nextGameState); 
        ///////////////////////////////////////////////////////////////////////////////////////////////
    }

    private void DestroyCombos()
    {
        RedCandiesDestroyer?.Invoke();

        BlueCandiesDestroyer?.Invoke();

        GreenCandiesDestroyer?.Invoke();

        YellowCandiesDestroyer?.Invoke();

        PurpleCandiesDestroyer?.Invoke();
    }

    private void ResetDestroyer()
    {
        RedCandiesDestroyer = null;
        BlueCandiesDestroyer = null;
        GreenCandiesDestroyer = null;
        YellowCandiesDestroyer = null;
        PurpleCandiesDestroyer = null;
    }
}
