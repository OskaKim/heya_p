using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTouchHandler : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    private void Update()
    {
        // todo : test get tile
        if (Input.GetKeyDown(KeyCode.F))
        {
            var clickedPosition = new Vector3Int(0, -1, 0);
            tilemap.SetTile(clickedPosition, null);
        }
    }
}
