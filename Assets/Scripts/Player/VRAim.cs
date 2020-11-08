using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAim : MonoBehaviour
{
    public GameObject gunObject;
    public GameObject zoomHand;

    private Vector3 zoomAimOrigin = Vector3.zero;
    private Quaternion zoomAimRot = Quaternion.identity;

    private Vector3 restoreAimOrigin = Vector3.zero;
    private Quaternion restoreAimRot = Quaternion.identity;

    private Vector3 aimHandOrigin = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        restoreAimOrigin = gunObject.transform.localPosition;
        restoreAimRot = gunObject.transform.localRotation;
    }

    void Update()
    {
        if (Input.GetButton("Fire3Joy")) {
            gunObject.transform.localPosition = restoreAimOrigin;
            gunObject.transform.localRotation = restoreAimRot;
            Vector3 zoomHandNow = transform.InverseTransformPoint(zoomHand.transform.position);
            gunObject.transform.rotation *= Quaternion.AngleAxis((aimHandOrigin.x - zoomHandNow.x)*-45.0f, Vector3.up);
            gunObject.transform.rotation *= Quaternion.AngleAxis((aimHandOrigin.y - zoomHandNow.y) * 30.0f, Vector3.right);
        }

        if (Input.GetButtonDown("Fire3Joy")) {
            aimHandOrigin = transform.InverseTransformPoint(zoomHand.transform.position);
            zoomAimOrigin = gunObject.transform.position;
            zoomAimRot = gunObject.transform.rotation;
        } else if (Input.GetButtonUp("Fire3Joy")) {
            gunObject.transform.localPosition = restoreAimOrigin;
            gunObject.transform.localRotation = restoreAimRot;
        }
    }
}
