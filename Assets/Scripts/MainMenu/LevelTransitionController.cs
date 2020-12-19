using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransitionController : MonoBehaviour
{
    public float FinalParticleSimulationSpeed = 10;
    public float FinalParticleXScale = 10;
    public float FinalParticleYScale = 10;
    public float FinalTitleXScale = 10;
    public float FinalTitleYScale = 0.1f;
    public int TransitionTimeInSeconds = 1;
    public ParticleSystem TimeParticles;
    public Transform Title;
    public float cameraShakeMagnitudeAcceleration = 0.05f;

    private float initialParticleSimulationSpeed;
    private float initialParticleXScale;
    private float initialParticleYScale;
    private float initialTitleXScale;
    private float initialTitleYScale;
    private bool inTransition = false;
    private string nextSceneName;
    private Image fullScreenRectangle;

    private Vector3 originalCameraPosition;
    private bool nextCameraShakeActionIsDisplacement = true;
    private float cameraShakeMagnitude = 0f;

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
        if (inTransition)
        {
            UpdateForTransitionEffects();
        }

        if (IsFadedOut())
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void UpdateForTransitionEffects()
    {
        var mainModule = TimeParticles.main;
        mainModule.simulationSpeed += CalculateTransitionEffectValueDelta(FinalParticleSimulationSpeed, initialParticleSimulationSpeed);

        var scale = TimeParticles.shape.scale;
        scale.x += CalculateTransitionEffectValueDelta(FinalParticleXScale, initialParticleXScale);
        scale.y += CalculateTransitionEffectValueDelta(FinalParticleYScale, initialParticleYScale);
        var shape = TimeParticles.shape;
        shape.scale = scale;

        var titleLocalScale = Title.localScale;
        titleLocalScale.x += CalculateTransitionEffectValueDelta(FinalTitleXScale, initialTitleXScale);
        titleLocalScale.y += CalculateTransitionEffectValueDelta(FinalTitleYScale, initialTitleYScale);
        Title.localScale = titleLocalScale;

        ShakeCamera();
    }

    private void ShakeCamera()
    {
        if (nextCameraShakeActionIsDisplacement)
        {
            var localPosition = Camera.main.transform.localPosition;
            localPosition.x += Random.Range(-cameraShakeMagnitude, cameraShakeMagnitude);
            localPosition.y += Random.Range(-cameraShakeMagnitude, cameraShakeMagnitude);
            Camera.main.transform.localPosition = localPosition;
            cameraShakeMagnitude += cameraShakeMagnitudeAcceleration;
        }
        else
        {
            Camera.main.transform.localPosition = originalCameraPosition;
        }

        nextCameraShakeActionIsDisplacement = !nextCameraShakeActionIsDisplacement;
    }

    private float CalculateTransitionEffectValueDelta(float final, float initial)
    {
        return (final - initial) / TransitionTimeInSeconds * Time.deltaTime;
    }

    private bool IsFadedOut()
    {
        return fullScreenRectangle is Image && fullScreenRectangle.canvasRenderer.GetAlpha() == 1;
    }

    private void TransitionToLevel(string sceneName, TimePeriod timePeriod)
    {
        if (inTransition) return;

        inTransition = true;
        PrepareForTransitionEffects();
        nextSceneName = sceneName;
        TimeCannonPlayerPrefs.SetTimePeriod(timePeriod);
        FadeOut();
    }

    private void PrepareForTransitionEffects()
    {
        initialParticleSimulationSpeed = TimeParticles.main.simulationSpeed;
        initialParticleXScale = TimeParticles.shape.scale.x;
        initialParticleYScale = TimeParticles.shape.scale.y;
        initialTitleXScale = Title.localScale.x;
        initialTitleYScale = Title.localScale.y;
        var colorOverLifeTime = TimeParticles.colorOverLifetime;
        colorOverLifeTime.enabled = false;

        originalCameraPosition = Camera.main.transform.localPosition;
    }

    private void FadeOut()
    {
        GameObject blackRectangleContainer = new GameObject();
        this.fullScreenRectangle = blackRectangleContainer.AddComponent<Image>();
        blackRectangleContainer.transform.SetParent(this.transform);
        this.fullScreenRectangle.color = new Color32(255, 255, 255, 255);
        this.fullScreenRectangle.canvasRenderer.SetAlpha(0);

        var rectTransform = blackRectangleContainer.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);

        blackRectangleContainer.SetActive(true);
        fullScreenRectangle.CrossFadeAlpha(1, TransitionTimeInSeconds, false);
    }
}