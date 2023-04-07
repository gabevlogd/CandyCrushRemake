using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillManager : MonoBehaviour
{
    public List<Tile>[] TilesToRefill;

    private void OnEnable()
    {
        TilesToRefill = new List<Tile>[GridManager.Instance.MaxColumn];
    }
}
