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
        StoreSelectedTile(eventData);
    }


    private void StoreSelectedTile(PointerEventData eventData)
    {
        if (!GridManager.PressedTiles[0])
        {
            eventData.pointerClick.TryGetComponent(out Tile pressedTile);
            GridManager.PressedTiles[0] = pressedTile;
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
