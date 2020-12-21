using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;

    [SerializeField] private float scaleDecay = 1f;
    [SerializeField] private float scaleIncreasePerPoint = 0.005f;

    private TextMeshProUGUI scoreLabel;
    private TextMeshProUGUI highScoreLabel;
    private Transform scoreLabelParent;
    private float scaleFactor = 1;
    private int scoreNow = 0;

    private const int MinScale = 1;

    void Start()
    {
        instance = this;
        scoreLabel = GameObject.Find("Score Label").GetComponent<TextMeshProUGUI>();
        highScoreLabel = GameObject.Find("High Score Label").GetComponent<TextMeshProUGUI>();
        scoreLabelParent = scoreLabel.transform.parent;

        AddScore(0);

        string levelName = SceneManager.GetActiveScene().name;
        int stage = PlayerPrefs.GetInt("stageNow", 0);
        stage = Mathf.Clamp(stage, 0, 4);
        TimePeriod time = (TimePeriod)stage;
        highScoreLabel.text = "High Score: " + HighScores.GetHighScore(levelName, time.ToString());
    }

    void Update()
    {
        AnimateLabel();
    }

    public void AddScore(int points)
    {
        scaleFactor += points * scaleIncreasePerPoint;
        if(scaleFactor > 5.0f) {
            scaleFactor = 5.0f;
        }
        scoreNow += points;
        scoreLabel.text = "Score: " + scoreNow;

        string levelName = SceneManager.GetActiveScene().name;
        int stage = PlayerPrefs.GetInt("stageNow", 0);
        stage = Mathf.Clamp(stage, 0, 4);
        TimePeriod time = (TimePeriod)stage;
        int savedHighScores = HighScores.GetHighScore(levelName, time.ToString());

        if (savedHighScores < scoreNow)
        {
            HighScores.SaveHighScore(levelName, time.ToString(), scoreNow);
            highScoreLabel.text = "High Score: " + HighScores.GetHighScore(levelName, time.ToString());
        }
    }

    private void AnimateLabel()
    {
        scaleFactor -= scaleDecay * Time.deltaTime;
        scaleFactor = scaleFactor < MinScale ? MinScale : scaleFactor;

        scoreLabelParent.transform.localScale = Vector3.one * scaleFactor;
    }
}
