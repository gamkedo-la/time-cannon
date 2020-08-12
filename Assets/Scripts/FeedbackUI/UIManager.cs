using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Player;
    private SetVROrNot setVROrNotScript;
    [SerializeField] private GameObject RoundSummaryUIWebGL;
    void Start()
    {
        setVROrNotScript = Player.GetComponent<SetVROrNot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!setVROrNotScript.forceVR)
            {
                RoundSummaryUIWebGL.GetComponent<FillRoundSummaryInfo>().DrawScreen();
                RoundSummaryUIWebGL.SetActive(true);
            }
            //else //TODO
            //{
            //    RoundSummaryUIVR.SetActive(true);
            //}

        }
    }
}
