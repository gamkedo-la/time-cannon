using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmMovement : MonoBehaviour
{
    public float oscPhase = 0.5f;
    public float oscAmt = 3.0f;
    public float oscRate = 0.3f;
    public float oscAng = 0.0f;
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        // randomizing while testing
        RandomAll();
    }

    void RandomAll()
    {
        oscPhase = Random.Range(0.0f, 1.0f);
        oscAmt = Random.Range(3.5f, 8.0f);
        oscRate = Random.Range(0.3f, 1.2f);
        oscAng = Random.Range(0.0f, 360.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPos + Quaternion.AngleAxis(oscAng, Vector3.right) *
                        Vector3.up * oscAmt * Mathf.Cos(TimeKeeper.instance.fakeTime * oscRate);
    }
}
