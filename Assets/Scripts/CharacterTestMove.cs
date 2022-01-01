using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTestMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var curPos = transform.localPosition;
        curPos.x += Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        curPos.y += Input.GetAxisRaw("Vertical") * Time.deltaTime;
        transform.localPosition = curPos;
    }
}
