using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
    public static TimeKeeper instance;

    public float fakeTime = 0.0f;
    public float fakeTimePace = 1.0f;
    public float fakeTimeDelta = 0.0f;

    public FloatSO RoundTime;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        fakeTimePace += Input.GetAxis("Horizontal") * 1.3f * Time.deltaTime;
        fakeTimePace += Input.GetAxis("TimeAxis") * -0.8f * Time.deltaTime;
        fakeTimePace = Mathf.Clamp(TimeKeeper.instance.fakeTimePace, -2.0f, 2.0f);

        float tempDampen = 1.0f;
        if(Input.GetKey(KeyCode.Space))
        {
            tempDampen = 0.1f;
        }
        fakeTimeDelta = fakeTimePace * Time.deltaTime * tempDampen;
        fakeTime += fakeTimeDelta;

        RoundTime.value += Time.deltaTime;
    }

    private void OnDrawGizmos() {
		if (instance == null) instance = this;
	}
}
