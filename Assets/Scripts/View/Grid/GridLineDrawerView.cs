using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace grid
{
    public class GridLineDrawerView : MonoBehaviour
    {
        [SerializeField] private GameObject GridLinePrefab;
        private List<GameObject> gridLineCaches = new List<GameObject>();
        public void DrawGridLine()
        {
            // note : 맵 크기 고정치
            var rangeX = TileMapDefinition.TileRange.Right - TileMapDefinition.TileRange.Left;
            var rangeY = TileMapDefinition.TileRange.Up - TileMapDefinition.TileRange.Down;
            var gridLineCount = rangeX * rangeY;

            if (gridLineCaches.Count < gridLineCount)
            {
                // todo : 맵 크기가 가변일시 재활용 가능한 구조로 개수
                foreach (var gridLineCache in gridLineCaches)
                {
                    GameObject.DestroyImmediate(gridLineCache);
                }
                gridLineCaches.Clear();
                gridLineCaches = new List<GameObject>(gridLineCount);
                for (int i = 0; i < gridLineCount; ++i)
                {
                    // note : 생성
                    var gridLine = new GameObject($"GridLine{i}");
                    gridLine.transform.SetParent(transform);
                    gridLineCaches.Add(gridLine);
                }
            }
            else
            {
                // note : 이미 있으므로 표시
                foreach (var gridLineCache in gridLineCaches)
                {
                    gridLineCache.SetActive(true);
                }
            }

            // todo : 해당하는 타일 위치에 그리드 라인을 그리기
        }

        public void HideGridLine()
        {
            foreach (var gridLineCache in gridLineCaches)
            {
                gridLineCache.SetActive(false);
            }
        }
    }
}