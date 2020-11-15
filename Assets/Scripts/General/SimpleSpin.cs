using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpin : MonoBehaviour
{
    public float rate = 10.0f;

    public enum spinAxis {onX,onY,onZ};

    public spinAxis spinOn = spinAxis.onX;

    private Vector3 onAxis;

    void Start() {
        switch (spinOn) {
            case spinAxis.onX:
                onAxis = Vector3.right;
                break;
            case spinAxis.onY:
                onAxis = Vector3.up;
                break;
            case spinAxis.onZ:
                onAxis = Vector3.forward;
                break;
        }
    }
    void Update() {
        transform.Rotate(onAxis, rate * TimeKeeper.instance.fakeTimeDelta);
    }
}
