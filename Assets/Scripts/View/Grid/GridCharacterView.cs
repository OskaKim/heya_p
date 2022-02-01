using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace grid
{
    public class GridCharacterView : MonoBehaviour
    {
        [SerializeField] private Vector3Int characterInitializePosition;
        [SerializeField] private Grid grid;
        [SerializeField] private GameObject characterPrefab;

        public void StartView()
        {
            var worldPos = grid.CellToWorld(characterInitializePosition);
            GameObject.Instantiate(characterPrefab, worldPos, Quaternion.identity);
        }
    }
}