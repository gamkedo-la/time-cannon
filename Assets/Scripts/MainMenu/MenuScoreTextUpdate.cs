using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuScoreTextUpdate : MonoBehaviour
{
    private readonly string[] sceneNames = { "Desert", "Ocean", "Countryside", "Chaos" };
    public TextMeshProUGUI[] sceneLabels;

    private readonly string[] periodTimes = { "450 BC", "1400 AD", "1942 AD", "2100 AD", "32000 AD" };
    public TextMeshProUGUI[] periodLabels_Desert;
    public TextMeshProUGUI[] periodLabels_Ocean;
    public TextMeshProUGUI[] periodLabels_Countryside;
    public TextMeshProUGUI[] periodLabels_Chaos;

    // Start is called before the first frame update
    void Start()
    {
        int[] sums = {0,0,0,0};
        int randScore;
        for (int i = 0; i < periodTimes.Length; i++) {
            randScore = Random.Range(0, 10) * 50 + 0;
            sums[0] += randScore;
            periodLabels_Desert[i].text = periodTimes[i] + "\nScore: "+ randScore;

            randScore = Random.Range(0, 10) * 50 + 0;
            sums[1] += randScore;
            periodLabels_Ocean[i].text = periodTimes[i] + "\nScore: "+randScore;

            randScore = Random.Range(0, 10) * 50 + 0;
            sums[2] += randScore;
            periodLabels_Countryside[i].text = periodTimes[i] + "\nScore: "+randScore;

            randScore = Random.Range(0, 10) * 50 + 0;
            sums[3] += randScore;
            periodLabels_Chaos[i].text = periodTimes[i] + "\nScore: "+randScore;
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
