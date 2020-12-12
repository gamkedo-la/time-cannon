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
        string mode = LevelMode.FreePosition.ToString();
        highScoreLabel.text = "High Score: " + HighScores.GetHighScore(levelName, mode);
    }

    void Update()
    {
        AnimateLabel();
    }

    public void AddScore(int points)
    {
        scaleFactor += points * scaleIncreasePerPoint;
        scoreNow += points;
        scoreLabel.text = "Score: "+ scoreNow;

        string levelName = SceneManager.GetActiveScene().name;
        string mode = LevelMode.FreePosition.ToString();
        int savedHighScores = HighScores.GetHighScore(levelName, mode);

        if (savedHighScores < scoreNow)
        {
            HighScores.SaveHighScore(levelName, mode, scoreNow);
            highScoreLabel.text = "High Score: " + HighScores.GetHighScore(levelName, mode);
        }
    }

    private void AnimateLabel()
    {
        scaleFactor -= scaleDecay * Time.deltaTime;
        scaleFactor = scaleFactor < MinScale ? MinScale : scaleFactor;

        scoreLabelParent.transform.localScale = Vector3.one * scaleFactor;
    }
}
