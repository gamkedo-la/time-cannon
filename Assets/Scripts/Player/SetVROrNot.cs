using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVROrNot : MonoBehaviour {
    public bool forceVR = false;

    public GameObject webController;
    public GameObject vrController;
    public GameObject postProcessingVolume;
    public bool disablePostProcessinginVR = false;

    void Awake() {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            forceVR = false;
        } else if (Application.platform == RuntimePlatform.Android) {
            forceVR = true;
        }
        vrController.SetActive(forceVR);
        webController.SetActive(!forceVR);

        if (forceVR && disablePostProcessinginVR)
        {
            if (postProcessingVolume != null)
            {
                postProcessingVolume.SetActive(false);
            }
            else
            {
                Debug.LogError("To disable post-processing in VR you have to attach it's volume to this script", gameObject);
            }
        }
    }
}