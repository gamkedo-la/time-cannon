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

		for (int i = 10; i > 0; i--)
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
