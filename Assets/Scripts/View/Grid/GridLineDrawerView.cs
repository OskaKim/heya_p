using UnityEngine;
using System.Collections.Generic;

namespace grid
{
    public class GridLineDrawerView : MonoBehaviour
    {
        [SerializeField] private GameObject GridLinePrefab;

        public void DrawGridLine(List<GameObject> targetTiles)
        {
            // todo : 해당하는 타일에 그리드 라인을 그리기
            Debug.Log(targetTiles);
        }
    }
}