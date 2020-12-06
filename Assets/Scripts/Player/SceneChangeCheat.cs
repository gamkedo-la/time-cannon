using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeCheat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("REMINDER: JOY/VR BUTTON LEVEL SKIP IS STILL ON");
    }

    // Update is called once per frame
    void Update()
    {
        // Fire5Joy lock slow mode from left middle finger controller
        // Fire6Joy was secondary middle finger of right controller
        if (Input.GetButtonDown("Fire4Joy")) {
            SceneManager.LoadScene(0); // menu
        }
        /*if (Input.GetButtonDown("Fire7Joy")) {
            // no action? button 7 might not be reachable by defualt Unity input mapping
        }*/
    }
}
