using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransitionController : MonoBehaviour
{
    public int TransitionTimeInSeconds = 1;

    private bool inTransition = false;
    private string nextSceneName;
    private Image fullScreenBlackRectangle;

    private void Start()
    {
        var buttonSceneAndTimePeriodSetters =  GetComponentsInChildren<ButtonSceneAndTimePeriodSetter>();

        foreach (var buttonSceneAndTimePeriodSetter in buttonSceneAndTimePeriodSetters)
        {
            var button = buttonSceneAndTimePeriodSetter.GetComponent<Button>();
            button.onClick.AddListener(() => TransitionToLevel(buttonSceneAndTimePeriodSetter.sceneName, buttonSceneAndTimePeriodSetter.timePeriod));
        }
    }

    private void Update()
    {
        if (IsFadedToBlack())
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private bool IsFadedToBlack()
    {
        return fullScreenBlackRectangle is Image && fullScreenBlackRectangle.canvasRenderer.GetAlpha() == 1;
    }

    private void TransitionToLevel(string sceneName, TimePeriod timePeriod)
    {
        if (inTransition) return;

        inTransition = true;
        nextSceneName = sceneName;
        TimeCannonPlayerPrefs.SetTimePeriod(timePeriod);
        FadeToBlack();
    }

    private void FadeToBlack()
    {
        GameObject blackRectangleContainer = new GameObject();
        this.fullScreenBlackRectangle = blackRectangleContainer.AddComponent<Image>();
        blackRectangleContainer.transform.SetParent(this.transform);
        this.fullScreenBlackRectangle.color = new Color32(0, 0, 0, 255);
        this.fullScreenBlackRectangle.canvasRenderer.SetAlpha(0);

        var rectTransform = blackRectangleContainer.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);

        blackRectangleContainer.SetActive(true);
        fullScreenBlackRectangle.CrossFadeAlpha(1, TransitionTimeInSeconds, false);
    }
}