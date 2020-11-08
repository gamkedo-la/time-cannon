using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAim : MonoBehaviour
{
    public GameObject gunObject;
    public GameObject zoomHand;

    public Camera zoomCam;
    public Transform zoomDot;

    private Vector3 zoomAimOrigin = Vector3.zero;
    private Quaternion zoomAimRot = Quaternion.identity;

    private Vector3 restoreAimOrigin = Vector3.zero;
    private Quaternion restoreAimRot = Quaternion.identity;
    private Transform restoreParent;

    private Vector3 aimHandOrigin = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        restoreAimOrigin = gunObject.transform.localPosition;
        restoreAimRot = gunObject.transform.localRotation;
        restoreParent = gunObject.transform.parent;
    }

    void LateUpdate()
    {
        if (Input.GetButtonDown("Fire5Joy")) {
            aimHandOrigin = transform.InverseTransformPoint(zoomHand.transform.position);
            zoomAimOrigin = gunObject.transform.position;
            zoomAimRot = gunObject.transform.rotation;
            gunObject.transform.SetParent(null);
        }

        if (Input.GetButton("Fire5Joy")) {
            gunObject.transform.position = zoomAimOrigin;
            gunObject.transform.rotation= zoomAimRot;
            Vector3 zoomHandNow = transform.InverseTransformPoint(zoomHand.transform.position);
            gunObject.transform.rotation *= Quaternion.AngleAxis((aimHandOrigin.x - zoomHandNow.x)*-20.0f, Vector3.up);
            gunObject.transform.rotation *= Quaternion.AngleAxis((aimHandOrigin.y - zoomHandNow.y) * 15.0f, Vector3.right);
        }

        if (Input.GetButtonUp("Fire5Joy")) {
            gunObject.transform.SetParent(restoreParent);
            gunObject.transform.localPosition = restoreAimOrigin;
            gunObject.transform.localRotation = restoreAimRot;
        }

        zoomCam.fieldOfView += Input.GetAxis("ZoomAxis") * Time.deltaTime * -6.0f;
        zoomCam.fieldOfView = Mathf.Clamp(zoomCam.fieldOfView,0.5f,10.0f);
        zoomDot.localScale = Vector3.one * zoomCam.fieldOfView / 8000.0f;
    }
}
