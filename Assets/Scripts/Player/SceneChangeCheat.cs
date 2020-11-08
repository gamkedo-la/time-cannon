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
            SceneManager.LoadScene(0);
        }
        if (Input.GetButtonDown("Fire4Joy")) {
            SceneManager.LoadScene(1);
        }
        if (Input.GetButtonDown("Fire5Joy")) {
            SceneManager.LoadScene(2);
        }
        if (Input.GetButtonDown("Fire6Joy")) {
            SceneManager.LoadScene(3);
        }
        if (Input.GetButtonDown("Fire7Joy")) {
            SceneManager.LoadScene(4);
        }
        if (Input.GetButtonDown("Fire8Joy")) {
            SceneManager.LoadScene(5);
        }
    }
}
