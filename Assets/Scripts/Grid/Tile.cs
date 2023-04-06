using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public TileData Data;

    public void Initialize(GridManager gridM, int rowInit, int columnInit)
    {
        Data = new TileData(gridM, rowInit, columnInit);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("OnPointerClickTile");
        if (GameManager.Instance.CurrentState.StateID == GameState.WaitMove) StoreSelectedTile(eventData);
    }

    /// <summary>
    /// Stores the tiles clicked by the player
    /// </summary>
    /// <param name="eventData"></param>
    private void StoreSelectedTile(PointerEventData eventData)
    {
        if (!GridManager.PressedTiles[0])
        {
            eventData.pointerClick.TryGetComponent(out Tile pressedTile);
            GridManager.PressedTiles[0] = pressedTile;
            pressedTile.GetComponentInChildren<Candy>().Animator.SetBool("Selected", true); //visual feedback of the selection
            return;
        }
        if (!GridManager.PressedTiles[1])
        {
            eventData.pointerClick.TryGetComponent(out Tile pressedTile);
            GridManager.PressedTiles[1] = pressedTile;
        }

        //Debug.Log("move complete");
    }
}
