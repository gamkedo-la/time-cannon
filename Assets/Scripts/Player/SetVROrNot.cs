using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVROrNot : MonoBehaviour {
    public bool forceVR = false;

    public GameObject webController;
    public GameObject vrController;

    void Awake() {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            forceVR = false;
        } else if (Application.platform == RuntimePlatform.Android) {
            forceVR = true;
        }
        vrController.SetActive(forceVR);
        webController.SetActive(!forceVR);
    }
}