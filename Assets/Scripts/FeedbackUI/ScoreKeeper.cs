using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;

    private TextMeshProUGUI scoreLabel;
    private int scoreNow = 0;

    void Start()
    {
        instance = this;
        scoreLabel = GameObject.Find("Score Label").GetComponent<TextMeshProUGUI>();
        AddScore(0);
    }

    public void AddScore(int points)
    {
        scoreNow += points;
        scoreLabel.text = "Score: "+ scoreNow;
    }
}
