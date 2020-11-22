using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuScoreTextUpdate : MonoBehaviour
{
    private readonly string[] sceneNames = { "Desert", "Ocean", "Countryside", "Chaos" };
    public TextMeshProUGUI[] sceneLabels;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< sceneLabels.Length; i++) {
            sceneLabels[i].text = sceneNames[i]+"\nTotal: 0";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
