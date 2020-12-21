using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuScoreTextUpdate : MonoBehaviour {
    public TextMeshProUGUI allScoreLabel;
    public Text winLabel;

    private readonly string[] sceneNames = { "Desert", "Ocean", "Countryside", "Chaos" };
    public TextMeshProUGUI[] sceneLabels;

    private readonly string[] periodTimes = { "450 BC", "1400 AD", "1942 AD", "2100 AD", "TIMELESS" };
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

    int[] sums = { 0, 0, 0, 0 };

    private void UpdateAllScore() {
        int totalSum = 0;
        for (int i = 0; i < sums.Length; i++) {
            totalSum += sums[i];
        }
        int goalScore = 5000;
        allScoreLabel.text = "Goal: "+ goalScore + "\nSum: " + totalSum;
        winLabel.text = (goalScore <= totalSum ?
        "You stopped the time aliens! Turns out you're the you in a timeline where the time aliens have always been stopped. Congratulations!" :
        "Control time to line up chain reactions for maximum score. Ammo limit varies 1-4 per stage. Time alien eyes explode once per row.");
    }

    public void ResetRowScore(int forRow) {
        int[] rowScores = ScorePerPeriod(forRow);
        TextMeshProUGUI[] labelRow = RowToLabelSet(forRow);
        sums[forRow] = 0;
        for (int i = 0; i < rowScores.Length; i++) {
            rowScores[i] = 0; //Random.Range(0, 10) * 50 + 0;
            sums[forRow] += rowScores[i];
            labelRow[i].text = periodTimes[i] + "\nScore: " + rowScores[i];
        }
        sceneLabels[forRow].text = sceneNames[forRow] + "\nTotal: " + sums[forRow];

        UpdateAllScore();

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

    void Start()
    {
        for(int i=0; i < sums.Length; i++) {
            sums[i] = 0;
        }

        for (int i = 0; i < periodTimes.Length; i++) {
            scores_Desert[i] = HighScores.GetHighScore(LevelName.City, (TimePeriod)i);
            sums[ROW_DESERT] += scores_Desert[i];
            periodLabels_Desert[i].text = periodTimes[i] + "\nScore: "+ scores_Desert[i];

            scores_Ocean[i] = HighScores.GetHighScore(LevelName.Ocean, (TimePeriod)i);
            sums[ROW_OCEAN] += scores_Ocean[i];
            periodLabels_Ocean[i].text = periodTimes[i] + "\nScore: "+ scores_Ocean[i];

            scores_Country[i] = HighScores.GetHighScore(LevelName.Countryside, (TimePeriod)i);
            sums[ROW_COUNTRY] += scores_Country[i];
            periodLabels_Countryside[i].text = periodTimes[i] + "\nScore: "+ scores_Country[i];

            scores_Chaos[i] = HighScores.GetHighScore(LevelName.ChaosDimension, (TimePeriod)i);
            sums[ROW_CHAOS] += scores_Chaos[i];
            periodLabels_Chaos[i].text = periodTimes[i] + "\nScore: "+ scores_Chaos[i];
        }

        for (int i = 0; i< sceneLabels.Length; i++) {
            sceneLabels[i].text = sceneNames[i]+"\nTotal: "+sums[i];
        }

        UpdateAllScore();
    }
}
