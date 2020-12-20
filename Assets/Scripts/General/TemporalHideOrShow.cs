using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalHideOrShow : MonoBehaviour
{
    public int[] timeStepsShownIn;

    private Color32 showColor = new Color32(0, 255, 0, 255);
    private Color32 hideColor = new Color32(255,0,0, 150);

    // didn't end up using, were prototyped for cause-effect changes between eras
    //public string[] reqAnyTrue;
    //public string[] reqAllFalse;

    bool debugMode = true;

    // -1: don't force/change/preview time
    // 0: 450 BC
    // 1: 1400 AD
    // 2: 1942 AD
    // 3: 2100 AD
    // 4: 32000 AD
    static int editorTestEra = -1; // set to -1 to show no era show/hide bubbles

    private void Awake() {
        if(editorTestEra != -1) {
            Debug.LogWarning("TemporalHideOrShow.editorTestEra is not -1, forcing era to " + editorTestEra);
        }
        PlayerPrefs.SetInt("stageNow", editorTestEra);
    }

    void ShouldntExistHereAndNow(string message) {
        if(debugMode) { // mark and hide
            gameObject.name = "HIDDEN " + gameObject.name;
            gameObject.SetActive(false);
        } else { // fully destroy it
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos() {
        if(editorTestEra == -1) {
            return;
        }
        bool shouldShow = false;
        for (int i = 0; i < timeStepsShownIn.Length; i++) {
            if (timeStepsShownIn[i] == editorTestEra) {
                shouldShow = true;
                break;
            }
        }
        Renderer[] rendChildren = GetComponentsInChildren<Renderer>();
        Gizmos.color = (shouldShow ? showColor : hideColor);
        for (int i = 0; i < rendChildren.Length; i++) { // pin where it is
            if(rendChildren[i].enabled) { // to not draw for waypoints etc.
                Gizmos.DrawSphere(rendChildren[i].transform.position+Vector3.up*90.0f, (shouldShow ? 10 : 5));
                Gizmos.DrawLine(rendChildren[i].transform.position + Vector3.up * 90.0f, rendChildren[i].transform.position);
                break; // only draw for one enabled renderer under children
            }
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
                ShouldntExistHereAndNow("removing " + gameObject.name + " - doesn't match timeStepsShownIn. stageNow="+stageNow);
                return;
            }
        } else {
            Debug.Log("time stage check bypass for " + gameObject.name + "(-1 value or unset)");
        }

        /*bool anyTrue = false;
        for (int i = 0; i < reqAnyTrue.Length; i++) {
            if (PlayerPrefs.GetInt(reqAnyTrue[i],0) == 1) {
                anyTrue = true;
                break;
            }
        }

        if (reqAnyTrue.Length==0) anyTrue = true; // don't fail check if nothing specified

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
        }*/
        if(debugMode) {
            gameObject.name = "EXISTS " + gameObject.name;
        }
        Debug.Log(gameObject.name + "Temporal checks passed, will exist here and now");
    }
}
