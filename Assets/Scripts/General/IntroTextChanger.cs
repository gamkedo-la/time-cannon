using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class IntroTextChanger : MonoBehaviour
{
    public Text changeThisText;

    [TextArea(3,10)]
    public string Intro_0 = "Year: 450 BC";
    [TextArea(3,10)]
    public string Intro_1 = "Year: 1400 AD";
    [TextArea(3,10)]
    public string Intro_2 = "Year: 1942 AD";
    [TextArea(3,10)]
    public string Intro_3 = "Year: 2100 AD";
    [TextArea(3,10)]
    public string Intro_4 = "Year: TIMELESS";
    
    // Start is called before the first frame update
    void Start()
    {

        GameObject IntroTextVR = GameObject.Find("IntroTextVR");

        if(IntroTextVR) {
            changeThisText = IntroTextVR.GetComponent<Text>();
        }

        if (!changeThisText) {
            Debug.Log("ERROR: Intro Text Changer does not have a reference to GUI Text we need to change.");
            return;
        }

        int stageNow = PlayerPrefs.GetInt("stageNow",-1);
        if (stageNow < 0) stageNow = 0;
        if (stageNow > 4) stageNow = 4;

        Debug.Log("Intro Text Changing to Time Stage " + stageNow);
        
        // fixme: make this a list of strings in the inspector
        // so the number of eras is not hardcoded
        // for now this is simple and works great lol
        if (stageNow==0) changeThisText.text = Intro_0;
        if (stageNow==1) changeThisText.text = Intro_1;
        if (stageNow==2) changeThisText.text = Intro_2;
        if (stageNow==3) changeThisText.text = Intro_3;
        if (stageNow==4) changeThisText.text = Intro_4;

        // Debug.Log("Intro Text will be: " + changeThisText.text);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
