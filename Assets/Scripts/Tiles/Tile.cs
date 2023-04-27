using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public TileData Data;

    public void Initialize(int rowInit, int columnInit)
    {
        Data = new TileData(rowInit, columnInit);
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
        if (!GameManager.Instance.GridManager.PressedTiles[0])
        {
            eventData.pointerClick.TryGetComponent(out Tile pressedTile);
            GameManager.Instance.GridManager.PressedTiles[0] = pressedTile;

            GridManager.GetCandy(pressedTile).Animator.SetBool("Selected", true); //visual feedback of the selection

            return;
        }
        if (!GameManager.Instance.GridManager.PressedTiles[1])
        {
            eventData.pointerClick.TryGetComponent(out Tile pressedTile);
            GameManager.Instance.GridManager.PressedTiles[1] = pressedTile;
        }

        //Debug.Log("move complete");
    }
}
