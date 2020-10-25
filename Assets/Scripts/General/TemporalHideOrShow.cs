using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalHideOrShow : MonoBehaviour
{
    public int[] timeStepsShownIn;

    public string[] reqAnyTrue;
    public string[] reqAllFalse;

    bool debugMode = true;

    void ShouldntExistHereAndNow(string message) {
        if(debugMode) { // mark and hide
            gameObject.name = "HIDDEN " + gameObject.name;
            gameObject.SetActive(false);
        } else { // fully destroy it
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        bool shouldShow = false;

        int stageNow = PlayerPrefs.GetInt("stageNow",-1); // -1 accepts any

        if (stageNow != -1) {
            for (int i = 0; i < timeStepsShownIn.Length; i++) {
                if (timeStepsShownIn[i] == stageNow) {
                    shouldShow = true;
                    break;
                }
            }
            if (shouldShow == false) {
                ShouldntExistHereAndNow("removing " + gameObject.name + " - doesn't match timeStepsShownIn");
                return;
            }
        } else {
            Debug.Log("time stage check bypass for " + gameObject.name + "(-1 value or unset)");
        }

        bool anyTrue = false;
        for (int i = 0; i < reqAnyTrue.Length; i++) {
            if (PlayerPrefs.GetInt(reqAnyTrue[i],0) == 1) {
                anyTrue = true;
                break;
            }
        }
        if (anyTrue == false) {
            ShouldntExistHereAndNow("removing " + gameObject.name + " - no reqAnyTrue matched");
            return;
        }

        anyTrue = false;
        for (int i = 0; i < reqAllFalse.Length; i++) {
            if (PlayerPrefs.GetInt(reqAllFalse[i],0) != 0) {
                anyTrue = true;
                break;
            }
        }
        if (anyTrue) {
            ShouldntExistHereAndNow("removing " + gameObject.name + " - any reqAllFalse matched");
            return;
        }
        if(debugMode) {
            gameObject.name = "EXISTS " + gameObject.name;
        }
        Debug.Log(gameObject.name + "Temporal checks passed, will exist here and now");
    }
}
