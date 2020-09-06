using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveWithTime : MonoBehaviour
{
    public float cycleRate = 1.0f;
    public float cycleOffset = 0.0f;
    Vector3 startScale;
    Vector3 startPos;
    Vector3 scaleBy = Vector3.one;
    Vector3 moveBy;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float cycleNow = (TimeKeeper.instance.fakeTime + cycleOffset)* cycleRate;
        moveBy = startPos;
        moveBy.x += 1.5f * Mathf.Cos(cycleNow * 3.0f);
        moveBy.z += 4.0f * Mathf.Cos(cycleNow * 1.7f);

        scaleBy = startScale;
        scaleBy.x *= 0.6f+(1.5f+Mathf.Cos(cycleNow * 2.0f))*0.25f;
        scaleBy.z *= 2.0f+Mathf.Cos(cycleNow * 5.1f );

        transform.localScale = scaleBy;
        transform.localPosition = moveBy;
    }
}
