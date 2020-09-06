using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// wobble movement - very similar to swarmMovement but with more variety

public class WobbleMovement : MonoBehaviour
{
    // each axis can have different speeds etc
    public float oscPhaseX = 0.5f;
    public float oscPhaseY = 0.5f;
    public float oscPhaseZ = 0.5f;
    public float oscAmtX = 3.0f;
    public float oscAmtY = 3.0f;
    public float oscAmtZ = 3.0f;
    public float oscRateX = 0.3f;
    public float oscRateY = 0.3f;
    public float oscRateZ = 0.3f;
    public float oscAngX = 0.0f;
    public float oscAngY = 0.0f;
    public float oscAngZ = 0.0f;
    public float spinX = 1.0f;
    public float spinY = 1.33f;
    public float spinZ = 3.14f;
    // optional random variations (+/- range added to the above)
    public float phaseVariance = 2f;
    public float amtVariance = 1.5f;
    public float rateVariance = 0.25f;
    public float angVariance = 180f;
    public float spinVariance = 1f;

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        RandomAll();
    }

    void RandomAll()
    {
        oscPhaseX += Random.Range(-phaseVariance,phaseVariance);
        oscAmtX += Random.Range(-amtVariance,amtVariance);
        oscRateX += Random.Range(-rateVariance,rateVariance);
        oscAngX += Random.Range(-angVariance,angVariance);
        oscPhaseY += Random.Range(-phaseVariance,phaseVariance);
        oscAmtY += Random.Range(-amtVariance,amtVariance);
        oscRateY += Random.Range(-rateVariance,rateVariance);
        oscAngY += Random.Range(-angVariance,angVariance);
        oscPhaseZ += Random.Range(-phaseVariance,phaseVariance);
        oscAmtZ += Random.Range(-amtVariance,amtVariance);
        oscRateZ += Random.Range(-rateVariance,rateVariance);
        oscAngZ += Random.Range(-angVariance,angVariance);
        spinX += Random.Range(-spinVariance,spinVariance);
        spinY += Random.Range(-spinVariance,spinVariance);
        spinZ += Random.Range(-spinVariance,spinVariance);
    }

    // Update is called once per frame
    void Update()
    {
        // FIXME this appears to not take startPos into account:
        // no matter where they start, they orbit the same place
        Vector3 tx = /*startPos +*/ Quaternion.AngleAxis(oscAngX, Vector3.right) *
                        Vector3.right * oscAmtX * Mathf.Cos(oscPhaseX + TimeKeeper.instance.fakeTime * oscRateX);
        Vector3 ty = /*startPos +*/ Quaternion.AngleAxis(oscAngY, Vector3.up) *
                        Vector3.up * oscAmtY * Mathf.Cos(oscPhaseY + TimeKeeper.instance.fakeTime * oscRateY);
        Vector3 tz = /*startPos +*/ Quaternion.AngleAxis(oscAngZ, Vector3.forward) *
                        Vector3.up * oscAmtZ * Mathf.Cos(oscPhaseZ + TimeKeeper.instance.fakeTime * oscRateZ);

        transform.position = new Vector3(startPos.x+tx.x,startPos.y+ty.y,startPos.z+tz.x);

        float spinSpeed = (TimeKeeper.instance.fakeTime/5);
        transform.Rotate(spinX*spinSpeed,spinY*spinSpeed,spinZ*spinSpeed);
    }
}
