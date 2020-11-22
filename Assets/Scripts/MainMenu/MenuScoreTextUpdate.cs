using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuScoreTextUpdate : MonoBehaviour {
    private readonly string[] sceneNames = { "Desert", "Ocean", "Countryside", "Chaos" };
    public TextMeshProUGUI[] sceneLabels;

    private readonly string[] periodTimes = { "450 BC", "1400 AD", "1942 AD", "2100 AD", "32000 AD" };
    public TextMeshProUGUI[] periodLabels_Desert;
    public TextMeshProUGUI[] periodLabels_Ocean;
    public TextMeshProUGUI[] periodLabels_Countryside;
    public TextMeshProUGUI[] periodLabels_Chaos;

    public int[] scores_Desert = { 0, 0, 0, 0, 0 };
    public int[] scores_Ocean = { 0, 0, 0, 0, 0 };
    public int[] scores_Country = { 0, 0, 0, 0, 0 };
    public int[] scores_Chaos = { 0, 0, 0, 0, 0 };

    const int ROW_DESERT = 0;
    const int ROW_OCEAN = 1;
    const int ROW_COUNTRY = 2;
    const int ROW_CHAOS = 3;

    public void ResetRowScore(int forRow) {
        int[] rowScores = ScorePerPeriod(forRow);
        TextMeshProUGUI[] labelRow = RowToLabelSet(forRow);
        int sum = 0;
        for (int i = 0; i < rowScores.Length; i++) {
            rowScores[i] = Random.Range(0, 10) * 50 + 0;
            sum += rowScores[i];
            labelRow[i].text = periodTimes[i] + "\nScore: " + rowScores[i];
        }
        sceneLabels[forRow].text = sceneNames[forRow] + "\nTotal: " + sum;

        int mostAliensPerScene = 15;
        Debug.Log("RESETTING ALIENS DEAD IN SCENE, REMINDER WE ASSUME NO MORE THAN " + mostAliensPerScene);
        for (int i = 0; i < mostAliensPerScene; i++) {
            PlayerPrefs.SetInt(StayDeadAcrossTimeScript.alienForRow(forRow,i), 1);
        }
    }

    TextMeshProUGUI[] RowToLabelSet(int forRow) {
        switch (forRow) {
            case ROW_DESERT:
                return periodLabels_Desert;
            case ROW_OCEAN:
                return periodLabels_Ocean;
            case ROW_COUNTRY:
                return periodLabels_Countryside;
            case ROW_CHAOS:
                return periodLabels_Chaos;
        }
        return null;
    }

    int[] ScorePerPeriod(int forRow) {
        switch(forRow) {
            case ROW_DESERT:
                return scores_Desert;
            case ROW_OCEAN:
                return scores_Ocean;
            case ROW_COUNTRY:
                return scores_Country;
            case ROW_CHAOS:
                return scores_Chaos;
        }
        Debug.LogWarning("invalid score row requested" + forRow);
        return null; // error
    }

    // Start is called before the first frame update
    void Start()
    {
        int[] sums = {0,0,0,0};
        for (int i = 0; i < periodTimes.Length; i++) {
            scores_Desert[i] = Random.Range(0, 10) * 50 + 0;
            sums[ROW_DESERT] += scores_Desert[i];
            periodLabels_Desert[i].text = periodTimes[i] + "\nScore: "+ scores_Desert[i];

            scores_Ocean[i] = Random.Range(0, 10) * 50 + 0;
            sums[ROW_OCEAN] += scores_Ocean[i];
            periodLabels_Ocean[i].text = periodTimes[i] + "\nScore: "+ scores_Ocean[i];

            scores_Country[i] = Random.Range(0, 10) * 50 + 0;
            sums[ROW_COUNTRY] += scores_Country[i];
            periodLabels_Countryside[i].text = periodTimes[i] + "\nScore: "+ scores_Country[i];

            scores_Chaos[i] = Random.Range(0, 10) * 50 + 0;
            sums[ROW_CHAOS] += scores_Chaos[i];
            periodLabels_Chaos[i].text = periodTimes[i] + "\nScore: "+ scores_Chaos[i];
        }

        for (int i = 0; i< sceneLabels.Length; i++) {
            sceneLabels[i].text = sceneNames[i]+"\nTotal: "+sums[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
