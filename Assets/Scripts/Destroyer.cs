using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
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
        PlayerScoresManager.PointsUpdateNeeded = true;
        DestroyCombos();
        this.enabled = false;
    }

    private void OnDisable()
    {
        ResetDestroyer();

        GameManager.Instance.ChangeState(GameState.RefillGrid); 
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
