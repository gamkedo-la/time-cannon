using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    public Waypoint target;

    private float progressAmt = 0.0f;

    void Start()
    {
        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;
    }

    void Update()
    {
        progressAmt += TimeKeeper.instance.fakeTimeDelta * target.speedMultiplierAfterHere;
        if (progressAmt <= 0.0f)
        {
            progressAmt += 1.0f;
            target = target.GetPrev();
        }
        if (progressAmt >= 1.0f)
        {
            progressAmt -= 1.0f;
            target = target.next;
        }
        transform.position = target.InterpPt(progressAmt);
        transform.rotation = target.InterpRot(progressAmt);
    }

    public void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
}
