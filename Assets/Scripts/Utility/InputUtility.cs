using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utility
{
    class InputUtility
    {
        // NOTE : 타일맵과 터치핸들러는 1:1관계
        private static Dictionary<Tilemap, TilemapTouchHandler> tilemapTouchHandlers = new Dictionary<Tilemap, TilemapTouchHandler>();

        // NOTE : targetTilemap의 터치를 담당하는 타일맵 터치 핸들러를 취득
        public static TilemapTouchHandler GetTilemapTouchHandler(Tilemap targetTilemap)
        {
            TilemapTouchHandler targetTilemapTouchHandler;
            if (!tilemapTouchHandlers.TryGetValue(targetTilemap, out targetTilemapTouchHandler))
            {
                // NOTE : 해당 타일맵의 터치핸들러가 존재하지 않으면 새로 만든다
                GameObject targetTilemapTouchHandlerObject = new GameObject($"{targetTilemap.name}TouchHandler");
                targetTilemapTouchHandler = TilemapTouchHandler.CreateComponent(targetTilemapTouchHandlerObject, targetTilemap);
            }
            return targetTilemapTouchHandler;
        }
    }
}