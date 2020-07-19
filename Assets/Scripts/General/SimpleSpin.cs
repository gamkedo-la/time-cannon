using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpin : MonoBehaviour
{
    public float rate = 10.0f;
    void Update()
    {
        transform.Rotate(Vector3.right, rate * TimeKeeper.instance.fakeTimeDelta);
    }
}
