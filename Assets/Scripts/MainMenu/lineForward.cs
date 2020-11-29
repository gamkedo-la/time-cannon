using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineForward : MonoBehaviour
{
    LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position+transform.forward*300.0f);
    }
}
