using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
	public Waypoint target;
	public float speedScale = 1f; // scales all waypoint speeds

	private float progressAmt = 0.0f;

	void Start()
	{
		transform.position = target.transform.position;
		transform.rotation = target.transform.rotation;

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
		progressAmt += TimeKeeper.instance.fakeTimeDelta * target.speedMultiplierAfterHere * speedScale;
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

	private bool initialized = false;
	[HideInInspector]
	public bool drawLingGizmo = true;
	[HideInInspector]
	public bool drawMeshGizmo = true;
	[HideInInspector]
	public bool drawFauxLocation = false;
	[HideInInspector]
	public float gizmoSize = 2.0f;
	public float fauxTime = 0.0f;
	public float oldFauxTime = 0.0f;
	public float fauxTimeScaled = 0.0f;
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

		if (fauxTime != oldFauxTime || fauxTimeScaled > 1f || fauxTimeScaled < 0f )
		{
			float fauxDiff = fauxTime - oldFauxTime;

			if (fauxDiff >= 1f)
			{
				fauxTimeScaled += 1f * tempTarget.speedMultiplierAfterHere * speedScale;
				oldFauxTime += 1f;
			}
			else if (fauxDiff <= -1f)
			{
				fauxTimeScaled -= 1f * tempTarget.speedMultiplierAfterHere * speedScale;
				oldFauxTime -= 1f;
			}
			else if (fauxDiff > 0f)
			{
				fauxTimeScaled += fauxDiff * tempTarget.speedMultiplierAfterHere * speedScale;
				oldFauxTime += fauxDiff;
			}
			else if (fauxDiff < 0f)
			{
				fauxTimeScaled -= fauxDiff * tempTarget.speedMultiplierAfterHere * speedScale;
				oldFauxTime -= fauxDiff;
			}

			if (fauxTimeScaled <= 0f)
			{
				fauxTimeScaled += 1f;
				tempTarget = tempTarget.GetPrev();
			}
			else if (fauxTimeScaled >= 1f)
			{
				fauxTimeScaled -= 1f;
				tempTarget = tempTarget.next;
			}
			if (fauxTime == 0f)
			{
				oldFauxTime = 0f;
				fauxTimeScaled = 0f;
				tempTarget = target;
			}
		}

		if (target != null && drawFauxLocation)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(tempTarget.InterpPt(fauxTimeScaled), gizmoSize);
		}
	}
}
