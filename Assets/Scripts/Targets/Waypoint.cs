using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint next;
    public float speedMultiplierAfterHere = 1.0f;
    private Waypoint prev; // set semi-automatically at start, by waypoints reporting in to their next

    void Start()
    {
        next.SetPrev(this);
    }

    public void SetPrev(Waypoint newPrev)
    {
        prev = newPrev;
    }
    public Waypoint GetPrev()
    {
        return prev;
    }

    public Vector3 InterpPt(float atT, float anchorSmoothDist = 6.0f)
    {
        if(next == null)
        {
            return transform.position;
        }
        Vector3 anchorFront = transform.position + transform.forward * anchorSmoothDist;
        Vector3 anchorEnd = next.transform.position - next.transform.forward * anchorSmoothDist;
        float antiT = 1.0f - atT;
        Vector3 termA = Mathf.Pow(antiT, 3) * transform.position;
        Vector3 termB = 3.0f * Mathf.Pow(antiT, 2) * atT * anchorFront;
        Vector3 termC = 3.0f * (antiT) *Mathf.Pow(atT, 2) * anchorEnd;
        Vector3 termD = Mathf.Pow(atT, 3) * next.transform.position;
        return termA + termB + termC + termD;
    }

    public Quaternion InterpRot(float atT)
    {
        return Quaternion.Slerp(transform.rotation, next.transform.rotation, atT);
    }

	[HideInInspector]
	public bool drawLineGizmo = true;
	[HideInInspector]
	public bool drawMeshGizmo = false;
	public void OnDrawGizmos()
    {
		next.SetPrev(this);

		if (next != null && drawLineGizmo)
        {
            Gizmos.color = Color.red;
            Vector3 prevPt = transform.position;
            for(int i=0;i<20;i++)
            {
                Vector3 nextPt = InterpPt((i + 1.0f) / 20.0f);
                Gizmos.DrawLine(prevPt, nextPt);
                prevPt = nextPt;
            }
        }

		gameObject.GetComponent<MeshRenderer>().enabled = drawMeshGizmo;
    }
}
