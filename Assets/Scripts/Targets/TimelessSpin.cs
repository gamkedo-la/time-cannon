using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelessSpin : MonoBehaviour
{
    void Update()
    {
        transform.rotation =
            Quaternion.AngleAxis(Time.timeSinceLevelLoad * 100.0f, Vector3.up) *
            Quaternion.AngleAxis(Time.timeSinceLevelLoad * 73.0f, Vector3.right) *
            Quaternion.AngleAxis(Time.timeSinceLevelLoad * 30.0f, Vector3.forward);
    }
}
