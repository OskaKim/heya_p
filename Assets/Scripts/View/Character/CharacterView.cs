using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private GameObject characterGameObject;

    public Vector3 GetCharacterUIPosition() {
        return Camera.main.WorldToScreenPoint(characterGameObject.transform.position);
    }
}
