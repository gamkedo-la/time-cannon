using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAnim : MonoBehaviour
{
    void Update()
    {
        transform.rotation = transform.parent.rotation * Quaternion.AngleAxis(-100.0f * TimeKeeper.instance.fakeTime, Vector3.up);
    }
}
