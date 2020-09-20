using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
	public Waypoint target;
    private Waypoint firstTarget;
    public float speedScale = 1f; // scales all waypoint speeds
    public bool driveSharpTurns = false;
    public bool waypointsMayMove = false;
    public bool isWater = false; // only used if driveSharpTurns, for driftier (but not air drifty) turn

    public GameObject vanishOnNextLapIfThisGORemoved;
    private bool watchingAnyGOForRemoval;

    private float progressAmt = 0.0f;

    private bool lateStartRunYet = false;

    void LateStart()
	{
        firstTarget = target;
        watchingAnyGOForRemoval = (vanishOnNextLapIfThisGORemoved != null);

        transform.position = target.transform.position;
		transform.rotation = target.transform.rotation;

        if(driveSharpTurns) { // force each point to point directly at the next waypoint
            Waypoint tracePt = target;

            if (isWater == false) { // if this is on ground...
                do { // drop each to a bit off the ground
                    RaycastHit rhInfo;
                    // 100.0f margin is in case path waypoint is accidentally a bit underground
                    if (Physics.Raycast(tracePt.transform.position + Vector3.up * 100.0f, Vector3.down, out rhInfo, 150.0f)) {
                        tracePt.transform.position = rhInfo.point + Vector3.up * 1.5f;
                    }
                    tracePt = tracePt.next;
                } while (firstTarget != tracePt);
            }

            float totalPathDist = 0.0f;
            do { // tally total path length, to adjust distance for consistent speed
                totalPathDist += Vector3.Distance(tracePt.transform.position, tracePt.next.transform.position);
                tracePt = tracePt.next;
            } while (firstTarget != tracePt);

            do { // remap proportions of path length for consistent ground speed
                tracePt.speedMultiplierAfterHere = totalPathDist / Vector3.Distance(tracePt.transform.position, tracePt.next.transform.position);
                tracePt = tracePt.next;
            } while (firstTarget != tracePt);

            do { // point each at the next
                if(isWater) {
                    Quaternion outQuat = Quaternion.LookRotation(tracePt.next.transform.position - tracePt.transform.position);
                    Quaternion inQuat = Quaternion.LookRotation(tracePt.transform.position - tracePt.GetPrev().transform.position);
                    tracePt.transform.rotation = Quaternion.Slerp(outQuat,inQuat,0.35f); // % aiming to next pt in water
                } else {
                    tracePt.transform.LookAt(tracePt.next.transform);
                }
                tracePt = tracePt.next;
            } while (firstTarget != tracePt);
        }

        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
		MeshCollider[] colliders = gameObject.GetComponentsInChildren<MeshCollider>();
		foreach (MeshRenderer renderer in renderers)
		{
			renderer.enabled = true;
		}
		foreach (MeshCollider collider in colliders)
		{
			collider.enabled = true;
		}
	}

	void Update()
	{
        if(lateStartRunYet == false) {
            LateStart(); // ensures all waypoint prev references set
            lateStartRunYet = true;
            return;
        }
        progressAmt += TimeKeeper.instance.fakeTimeDelta * target.speedMultiplierAfterHere * speedScale;
        bool loopRestarted = false; // true if either forward or backward crossing of start
		if (progressAmt <= 0.0f)
		{
			progressAmt += 1.0f;
			target = target.GetPrev();
            loopRestarted = true;
		}
		if (progressAmt >= 1.0f)
		{
			progressAmt -= 1.0f;
			target = target.next;
            loopRestarted = true;
        }
        if(loopRestarted && watchingAnyGOForRemoval && vanishOnNextLapIfThisGORemoved == null && target == firstTarget) {
            Debug.Log("Removing " + gameObject.name + " because crossing between lap sand its vanishOnNextLapIfThisGORemoved is gone");
            Destroy(gameObject); // silent, uneventful removal
        }
        if (isWater) {
            transform.position = target.InterpPt(progressAmt,40.0f);
        } else {
            transform.position = target.InterpPt(progressAmt);
        }
        if(waypointsMayMove) {
            target.transform.LookAt(target.next.transform); // update current wp
            target.next.transform.LookAt(target.next.next.transform); // and our destination's orientation
        }
        transform.rotation = target.InterpRot(driveSharpTurns || waypointsMayMove ?
            progressAmt* progressAmt* (isWater ? 1 : progressAmt * progressAmt * progressAmt * progressAmt) : // heavy, heavy bias towards end
            progressAmt);
	}

	private bool initialized = false;
	[HideInInspector]
	public bool drawLingGizmo = true;
	[HideInInspector]
	public bool drawMeshGizmo = true;
	[HideInInspector]
	public bool drawFauxLocation = false;
	[HideInInspector]
	public float gizmoSize = 2.0f;
	public double fauxTime = 0.0f;
	public double oldFauxTime = 0.0f;
	public double fauxTimeScaled = 0.0f;
	private Waypoint tempTarget;
	public void OnDrawGizmos()
	{
		if (!initialized)
		{
			tempTarget = target;
			initialized = true;
		}

		if (target != null && drawLingGizmo)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, target.transform.position);
		}

		MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
		MeshCollider[] colliders = gameObject.GetComponentsInChildren<MeshCollider>();
		foreach (MeshRenderer renderer in renderers)
		{
			renderer.enabled = drawMeshGizmo;
		}
		foreach (MeshCollider collider in colliders)
		{
			collider.enabled = drawMeshGizmo;
		}

		for (int i = 100; i > 0; i--)
		{
			if (fauxTime != oldFauxTime && fauxTimeScaled <= 1 && fauxTimeScaled >= 0)
			{
				double fauxDiff = fauxTime - oldFauxTime;

				if (fauxDiff >= 1)
				{
					fauxTimeScaled += 1 * tempTarget.speedMultiplierAfterHere * speedScale;
					oldFauxTime += 1;
				}
				else if (fauxDiff <= -1)
				{
					fauxTimeScaled -= 1 * tempTarget.speedMultiplierAfterHere * speedScale;
					oldFauxTime -= 1;
				}
				else if (fauxDiff > 0)
				{
					fauxTimeScaled += fauxDiff * tempTarget.speedMultiplierAfterHere * speedScale;
					oldFauxTime += fauxDiff;
				}
				else if (fauxDiff < 0)
				{
					fauxTimeScaled -= fauxDiff * tempTarget.speedMultiplierAfterHere * speedScale;
					oldFauxTime -= fauxDiff;
				}
			}

			if (fauxTimeScaled > 1 || fauxTimeScaled < 0)
			{
				if (fauxTimeScaled <= 0)
				{
					fauxTimeScaled += 1;
					tempTarget = tempTarget.GetPrev();
				}
				else if (fauxTimeScaled >= 1)
				{
					fauxTimeScaled -= 1;
					tempTarget = tempTarget.next;
				}
			}

			if (fauxTime == 0)
			{
				oldFauxTime = 0;
				fauxTimeScaled = 0;
				tempTarget = target;
			}
		}

		if (target != null && drawFauxLocation)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(tempTarget.InterpPt((float)fauxTimeScaled), gizmoSize);
		}
	}
}
