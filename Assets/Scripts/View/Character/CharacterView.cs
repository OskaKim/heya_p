using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private GameObject characterViewObject;

    public Vector3 GetCharacterUIPosition() {
        return Camera.main.WorldToScreenPoint(characterViewObject.transform.position);
    }

    public void UpdateCharacterPos(Vector3 pos) {
        characterViewObject.transform.position = pos;
    }
}
