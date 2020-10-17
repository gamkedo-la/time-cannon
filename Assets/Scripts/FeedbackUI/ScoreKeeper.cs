using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;

    [SerializeField] private float scaleDecay = 1f;
    [SerializeField] private float scaleIncreasePerPoint = 0.005f;

    private TextMeshProUGUI scoreLabel;
    private Transform scoreLabelParent;
    private float scaleFactor = 1;
    private int scoreNow = 0;

    private const int MinScale = 1;

    void Start()
    {
        instance = this;
        scoreLabel = GameObject.Find("Score Label").GetComponent<TextMeshProUGUI>();
        scoreLabelParent = scoreLabel.transform.parent;
        AddScore(0);
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
    }

    private void AnimateLabel()
    {
        scaleFactor -= scaleDecay * Time.deltaTime;
        scaleFactor = scaleFactor < MinScale ? MinScale : scaleFactor;

        scoreLabelParent.transform.localScale = Vector3.one * scaleFactor;
    }
}
