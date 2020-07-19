using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;

    private Text scoreText;
    private int scoreNow = 0;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        AddScore(0);
    }

    public void AddScore(int points)
    {
        scoreNow += points;
        scoreText.text = "Score: "+ scoreNow;
    }
}
