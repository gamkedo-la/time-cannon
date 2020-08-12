using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillRoundSummaryInfo : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject timePassedText;
    [SerializeField] private GameObject numberOfHitsText;

    [SerializeField] private FloatSO timePassed;
    [SerializeField] private IntSO totalNumberOfHits;
    [SerializeField] private IntListSO numberOfHitsList;
    [SerializeField] private IntListSO scoreFromEachShot;

    private void Update()
    {
        Time.timeScale = 0;
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManagerScript.Instance.RestartScene();
        }
    }
    public void DrawScreen()
    {

        timePassedText.GetComponent<Text>().text =  timePassed.value.ToString();
        numberOfHitsText.GetComponent<Text>().text = totalNumberOfHits.value.ToString();

        for (int i = 0; i < numberOfHitsList.value.Count; i++)
        {
            GameObject textObject = GameObject.Instantiate(textPrefab, Panel.transform);
            Text text = textObject.GetComponent<Text>();
            text.text = "Shoot - " + i + "===> number of hits = " + numberOfHitsList.value[i] + ", score = " + scoreFromEachShot.value[i];
        }
    }
}
