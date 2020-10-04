using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentInstantly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);   
    }
}
