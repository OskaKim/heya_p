using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TouchFurnitureTest : MonoBehaviour
{
    [SerializeField] private TilemapTouchHandler tilemapTouchHandler;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tile1;
    [SerializeField] private TileBase tile2;
    [SerializeField] private TileBase tile3;
    private TileBase currentTile;
    private void Start()
    {
        tilemapTouchHandler.OnTouchTilemap.Subscribe(touchedPosition => TouchTile(touchedPosition));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) currentTile = tile1;
        if (Input.GetKeyDown(KeyCode.W)) currentTile = tile2;
        if (Input.GetKeyDown(KeyCode.E)) currentTile = tile3;
        if (Input.GetKeyDown(KeyCode.R)) currentTile = null;
    }
    private void TouchTile(Vector3Int touchedPosition)
    {
        tilemap.SetTile(touchedPosition, currentTile);
    }
}
