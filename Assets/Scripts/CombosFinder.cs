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
        //if (RedCandiesDestroyer != null) yield return new WaitForSeconds(1);

        BlueCandiesDestroyer?.Invoke();
        //if (BlueCandiesDestroyer != null) yield return new WaitForSeconds(1);

        GreenCandiesDestroyer?.Invoke();
        //if (GreenCandiesDestroyer != null) yield return new WaitForSeconds(1);

        YellowCandiesDestroyer?.Invoke();
        //if (YellowCandiesDestroyer != null) yield return new WaitForSeconds(1);
    }

    private void ResetDestroyer()
    {
        RedCandiesDestroyer = null;
        BlueCandiesDestroyer = null;
        GreenCandiesDestroyer = null;
        YellowCandiesDestroyer = null;
    }
}
