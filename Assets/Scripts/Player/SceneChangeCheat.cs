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

        // debug controls to switch or reload scenes in VR
        if (Input.GetButtonDown("Fire3Joy")) {
            //SceneManager.LoadScene(0); // menu - no use loading in VR until working in VR
            // SceneManager.LoadScene(4); // chaos - error loading in VR? black screen
        }
        if (Input.GetButtonDown("Fire4Joy")) {
            SceneManager.LoadScene(1); // city
        }
        if (Input.GetButtonDown("Fire3Joy")) {
            SceneManager.LoadScene(2); // ocean
        }
        if (Input.GetButtonDown("Fire6Joy")) {
            SceneManager.LoadScene(3); // countryside
        }
        if (Input.GetButtonDown("Fire7Joy")) {
            // no action? button 7 might not be reachable by defualt Unity input mapping
        }
    }
}
