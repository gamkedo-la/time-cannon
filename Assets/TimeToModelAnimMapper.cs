using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToModelAnimMapper : MonoBehaviour
{
    public string animStateName = "TestAnim";
    public float timePerPingPong = 5.0f; // seconds per forward/backward cycles?
    private Animator forAnim;

    // Start is called before the first frame update
    void Start()
    {
        forAnim = GetComponent<Animator>();
        forAnim.speed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float fakeTimePingPong0to1;
        float dialatedLoopingAnimTime = TimeKeeper.instance.fakeTime / timePerPingPong; // seconds per forward/backward cycles?

        int truncatedInt = Mathf.FloorToInt(dialatedLoopingAnimTime);
        if (truncatedInt % 2 == 0) {
            fakeTimePingPong0to1 = dialatedLoopingAnimTime - (float)(truncatedInt);
        } else {
            fakeTimePingPong0to1 = (float)(truncatedInt + 1) - dialatedLoopingAnimTime;
        }

        forAnim.Play(animStateName, 0, fakeTimePingPong0to1);
    }
}
