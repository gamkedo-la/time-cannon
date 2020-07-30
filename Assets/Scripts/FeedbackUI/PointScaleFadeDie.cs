using TMPro;
using UnityEngine;

public class PointScaleFadeDie : MonoBehaviour
{
    [SerializeField] private Gradient color;
    private float scaleMax = 3f;
    private float initialScale = 1.0f;
    private float randLateralDrift = 0.0f;
    private TextMeshProUGUI myLabel = null;

    void Awake()
    {
        initialScale = transform.localScale.x;
        myLabel = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetText(int points,float scaleAdjustment)
    {
        myLabel.text = points.ToString();

        float maxPoints = 1500;
        myLabel.color = color.Evaluate(points / maxPoints);

        if(scaleAdjustment>1.0f)
        {
            scaleAdjustment -= 1.0f;
            scaleAdjustment *= 0.3f; // was getting too huge too fast, so dampening
            scaleAdjustment += 1.0f;
        }

        initialScale *= scaleAdjustment;
        scaleMax *= scaleAdjustment;
        randLateralDrift = Random.Range(-4.0f, 4.0f);

        transform.localScale = Vector3.one * initialScale;
    }

    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * scaleMax * 6f +
                                transform.right * Time.deltaTime * randLateralDrift * 5f;
        scaleMax -= Time.deltaTime * 5f;
    }

    public void Done()
    {
        Destroy(gameObject);
    }
}
