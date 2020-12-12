using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class TimeKeeper : MonoBehaviour
{
    private bool IsUsingVR;

    public static TimeKeeper instance;

    public float fakeTime = 0.0f;
    public float fakeTimePace = 1.0f;
    public float fakeTimeDelta = 0.0f;

    public FloatSO RoundTime;

    public float VRTimeInputModifier = -0.8f;
    public float NonVRTimeInputModifier = 1.3f;
    public float NonVRTimeInputModifierOnHold = 8f;

    public ParticleSystem TimeSmoke;

    void Start()
    {
        instance = this;
        IsUsingVR = IsAnyXRDisplaySubsystemRunning();
    }

    void Update()
    {
        var isZoomedIn = Input.GetKey(KeyCode.Space) || Input.GetButton("Fire5Joy");
        var timeForce = GetTimeForce(isZoomedIn);
        fakeTimePace += timeForce * Time.deltaTime;
        fakeTimePace = Mathf.Clamp(TimeKeeper.instance.fakeTimePace, -2.0f, 2.0f);
        fakeTimeDelta = fakeTimePace * Time.deltaTime * (isZoomedIn ? 0.1f : 
            (Mathf.Abs(1.0f-Input.GetAxis("LeftTriggerAxis")) * 1f));
        fakeTime += fakeTimeDelta;
        RoundTime.value += Time.deltaTime;

        RegulateTimeSmoke(timeForce != 0);
    }

    private void OnDrawGizmos() {
		if (instance == null) instance = this;
	}

    private void RegulateTimeSmoke(bool isTimeForceApplied)
    {
        if (isTimeForceApplied && TimeSmoke.isStopped)
        {
            TimeSmoke.Play();
        }
        else if (!isTimeForceApplied && TimeSmoke.isPlaying)
        {
            TimeSmoke.Stop();
        }
    }

    private float GetTimeForce(bool isZoomedIn)
    {
        if (IsUsingVR)
        {
            return Input.GetAxis("TimeAxis") * VRTimeInputModifier;
        }

        var isAxisHeld = Mathf.Abs(Input.GetAxis("Horizontal")) == 1;
        var multiplier = isZoomedIn || !isAxisHeld ? NonVRTimeInputModifier : NonVRTimeInputModifierOnHold;

        return Input.GetAxis("Horizontal") * multiplier;
    }

    private bool IsAnyXRDisplaySubsystemRunning()
    {
        var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances(xrDisplaySubsystems);

        return xrDisplaySubsystems.Any(xrDisplaySubsystem => xrDisplaySubsystem.running);
    }
}