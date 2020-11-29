using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnWithMouseMoves : MonoBehaviour
{
    float offX = 0.0f, offY = 0.0f;
    void Update()
    {
        offX += 850.0f * Time.deltaTime * Input.GetAxisRaw("Mouse X");
        offY += -650.0f * Time.deltaTime * Input.GetAxisRaw("Mouse Y");

        transform.rotation = Quaternion.identity *
            Quaternion.AngleAxis(offX, Vector3.up) *
            Quaternion.AngleAxis(offY, Vector3.right);
    }
}
