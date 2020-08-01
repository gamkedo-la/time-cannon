using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAim : MonoBehaviour
{
    private float turnAng = 0.0f;
    private float tiltAng = 0.0f;

    private float turnLimit = 60.0f;
    private float tiltLimit = 40.0f;

    private float scanFOV = 60.0f;
    private float zoomFOV = 25.0f;
    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        turnAng += Input.GetAxis("Mouse X") * Time.deltaTime * 60.0f;
        tiltAng += Input.GetAxis("Mouse Y") * Time.deltaTime * -60.0f;

        turnAng = Mathf.Clamp(turnAng,-turnLimit, turnLimit);
        tiltAng = Mathf.Clamp(tiltAng, -tiltLimit, tiltLimit);

        transform.localRotation = Quaternion.AngleAxis(turnAng, Vector3.up) *
                            Quaternion.AngleAxis(tiltAng, Vector3.right);
    }

    private void FixedUpdate()
    {
        float springVal = 0.9f;
        float towardFOV;
        if (Input.GetKey(KeyCode.Space))
        {
            towardFOV = zoomFOV;
        }
        else
        {
            towardFOV = scanFOV;
        }
        mainCam.fieldOfView = mainCam.fieldOfView * springVal + towardFOV * (1.0f- springVal);
    }
}
