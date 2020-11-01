using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHover : MonoBehaviour
{
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Cos(TimeKeeper.instance.fakeTime*2.5f)*9.0f
            + Vector3.right * Mathf.Cos(TimeKeeper.instance.fakeTime * 1.1f) * 4.0f
            + Vector3.forward * Mathf.Cos(TimeKeeper.instance.fakeTime * 0.7f) * 4.0f;
        transform.rotation = Quaternion.AngleAxis(TimeKeeper.instance.fakeTime * 6000.0f, Vector3.up);
    }
}
