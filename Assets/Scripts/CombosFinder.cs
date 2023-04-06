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
        StartCoroutine(pippo());
    }

    private IEnumerator pippo()
    {
        RedCandiesDestroyer?.Invoke();
        yield return new WaitForSeconds(1);
        BlueCandiesDestroyer?.Invoke();
        yield return new WaitForSeconds(1);
        GreenCandiesDestroyer?.Invoke();
        yield return new WaitForSeconds(1);
        YellowCandiesDestroyer?.Invoke();
    }
}
