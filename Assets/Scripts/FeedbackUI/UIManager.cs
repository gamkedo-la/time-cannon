using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject Player;
    private SetVROrNot setVROrNotScript;
    [SerializeField] private FillRoundSummaryInfo endScreen;
    //private GameObject RoundSummaryUIWebGL;

    void Awake() // has to be Awake, to before FillRoundSummaryInfo turns self off in Start
    {
        Player = GameObject.FindWithTag("Player");
        setVROrNotScript = Player.GetComponent<SetVROrNot>();
        // Debug.Log(endScreen);
        // RoundSummaryUIWebGL = Player.GetComponentInChildren<UIManager>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!setVROrNotScript.forceVR)
            {
                endScreen.DrawScreen();
                endScreen.gameObject.SetActive(true);
            }
            //else //TODO
            //{
            //    RoundSummaryUIVR.SetActive(true);
            //}

        }
    }
}
