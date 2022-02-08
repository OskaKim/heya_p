using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterAIStatusUIView : MonoBehaviour
{
    [SerializeField] private Transform aiStatusUIHolder;
    [SerializeField] private TMP_Text aiStatusText;

    public Func<Vector3> GetCharacterScreenPosition;

    public void Update()
    {
        FollowCharacter();
    }

    private void FollowCharacter()
    {
        aiStatusUIHolder.position = GetCharacterScreenPosition();
    }
}
