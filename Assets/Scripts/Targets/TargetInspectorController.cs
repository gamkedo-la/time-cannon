using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInspectorController : MonoBehaviour
{
	private bool initialized = false;
	private Waypoint[] points;
	private WaypointMover[] movers;

	public float fauxTime = 0.0f;

	public bool PointsDrawLineGizmo = true;
	public bool PointsDrawMeshGizmo = false;

	public bool MoversDrawLingGizmo = true;
	public bool MoversDrawMeshGizmo = true;
	public bool MoversDrawFauxLocation = false;
	public float MoverGizmoSize = 2.0f;

	private void OnDrawGizmos() {
		if (!initialized) {
			points = FindObjectsOfType<Waypoint>();
			movers = FindObjectsOfType<WaypointMover>();
			initialized = true;
		}

		foreach (Waypoint point in points) {
			point.drawLineGizmo = PointsDrawLineGizmo;
			point.drawMeshGizmo = PointsDrawMeshGizmo;
		}

		foreach (WaypointMover mover in movers) {
			mover.drawLingGizmo = MoversDrawLingGizmo;
			mover.drawMeshGizmo = MoversDrawMeshGizmo;
			mover.drawFauxLocation = MoversDrawFauxLocation;
			mover.gizmoSize = MoverGizmoSize;
			mover.fauxTime = fauxTime;
		}
	}
}
