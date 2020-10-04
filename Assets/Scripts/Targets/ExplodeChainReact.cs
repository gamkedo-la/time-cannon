using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(LineRenderer))]
public class ExplodeChainReact : MonoBehaviour
{
    private GameObject blastVFX;
    private GameObject pointPopper;

    public bool isUnderwater = false;
    private readonly int[] exploList = { 5, 10, 50, 100, 300};

    private bool isExploded = false;
    public int blastRadius = 5;

    [HideInInspector]
    public bool alreadyCountedInChainForAimThisFrame = false;

    private int numSegments = 100;
    private LineRenderer lineRenderer;

    //UI Elements for round summary screen
    [SerializeField] private IntListSO ScoreList;
    [SerializeField] private IntListSO NumberOfTargetsList;
    [SerializeField] private IntSO TotalNumberOfTargetHits;
    private void Start()
    {
        int useSize = exploList[exploList.Length - 1]; // scale largest if no smaller fit found
        string vfx_name;
        for (int i = 0; i < exploList.Length; i++) {
            if (exploList[i] >= blastRadius) {
                useSize = exploList[i];
                break;
            }
        }
        vfx_name = "Explosion" + useSize + "m";
        blastVFX = Resources.Load(vfx_name) as GameObject;

        blastVFX.transform.localScale = Vector3.one * 0.108f * ((float)blastRadius);

        if (blastVFX == null)
        {
            Debug.LogWarning("Blast Radius needs to match an existing Resources/Explosion_m prefab");
            Debug.LogWarning("No Resources/Explosion"+ blastRadius+"m definition found for " +gameObject.name);
            Debug.LogWarning("defaulting to 5m explosion, sad :/");
            blastVFX = Resources.Load("Explosion5m") as GameObject;
        }
        pointPopper = Resources.Load("MultiplierPop") as GameObject;

        gameObject.AddComponent<LineRenderer>();
        CreateBlastPreviewRangeRenderer();
        lineRenderer.enabled = false;
    }

    public void Explode(int chainDepth)
    {
        if (isExploded)
        {
            return;
        }
        Waypoint[] allWPChildrenToUnparent = GetComponentsInChildren<Waypoint>();
        for (int i = 0; i < allWPChildrenToUnparent.Length; i++) {
            allWPChildrenToUnparent[i].transform.SetParent(null);
        }

        isExploded = true;
        StartCoroutine(ChainReact(chainDepth)); // compute chain reaction instantly, no lag
    }

    public void LateUpdate()
    {
        lineRenderer.enabled = alreadyCountedInChainForAimThisFrame;
        alreadyCountedInChainForAimThisFrame = false;
        UpdateBlastPreviewRangeRenderer();
    }

    public int HitsInRange()
    {
        if(alreadyCountedInChainForAimThisFrame)
        {
            return 0; // already visited - also avoids recusive/feedback lock up
        }

        int hitsThroughMe = 1; // counts itself
        alreadyCountedInChainForAimThisFrame = true; // will stop from revisiting itself

        Collider[] allNear = Physics.OverlapSphere(transform.position, blastRadius);

        for (int i = 0; i < allNear.Length; i++)
        {
            ExplodeChainReact ecrScriptNext = allNear[i].gameObject.GetComponentInParent<ExplodeChainReact>();
            if (ecrScriptNext && ecrScriptNext.alreadyCountedInChainForAimThisFrame == false)
            {
                hitsThroughMe += ecrScriptNext.HitsInRange();
            }
        }
        return hitsThroughMe;
    }

    IEnumerator ChainReact(int chainDepth)
    {
        Collider[] allNear = Physics.OverlapSphere(transform.position, blastRadius);

        for (int i = 0; i < allNear.Length; i++)
        {
            ExplodeChainReact ecrScript = allNear[i].gameObject.GetComponentInParent<ExplodeChainReact>();
            if (ecrScript && gameObject != allNear[i].gameObject) // can explode, and isn't self
            {
                Debug.Log("CHAIN ("+chainDepth+") from " + gameObject.name + " to " + allNear[i].gameObject.name);
                ecrScript.Explode(chainDepth+1);
            }
        }

        yield return new WaitForSeconds(chainDepth * Random.Range(0.1f, 0.4f)); // lag the explosion

        Destroy(gameObject);

        GameObject pointGO = GameObject.Instantiate(pointPopper, transform.position, transform.rotation);
        PointScaleFadeDie psfdScript = pointGO.GetComponent<PointScaleFadeDie>();
        int score = chainDepth * 100;
        psfdScript.SetText(score, chainDepth);
        ScoreKeeper.instance.AddScore(score);

        // UI Elements Handling
        TotalNumberOfTargetHits.value += 1;
        if (chainDepth == 1)
        {
            ScoreList.value.Add(score);
            NumberOfTargetsList.value.Add(chainDepth);
        }
        else if (chainDepth > 1)
        {
            ScoreList.value[ScoreList.value.Count -1] += score;
            NumberOfTargetsList.value[NumberOfTargetsList.value.Count - 1] += 1;
        }

        if (isUnderwater == false) {
            GameObject blastGO = GameObject.Instantiate(blastVFX, transform.position, transform.rotation);
        }
        /*
        if (isUnderwater) {
            ParticleSystemRenderer[] allPSR = blastGO.GetComponentsInChildren<ParticleSystemRenderer>();
            for (var i = 0; i < allPSR.Length; i++) {
                allPSR[i].material.SetColor("_Color", Color.blue);
                allPSR[i].material.SetColor("_EmissionColor", Color.cyan);
                if(allPSR[i].trailMaterial) {
                    allPSR[i].trailMaterial.SetColor("_Color", Color.blue);
                    allPSR[i].trailMaterial.SetColor("_EmissionColor", Color.cyan);
                }
            }
        }*/

        // Destroy(gameObject); //// wasn't reached here, trying moving it earlier...
    }

    // adapted from https://gamedev.stackexchange.com/questions/126427/draw-circle-around-gameobject-to-indicate-radius
    public void CreateBlastPreviewRangeRenderer()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        Color c1 = new Color(1.0f, 1.0f, 1.0f, 1);
        lineRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        lineRenderer.startColor = c1;
        lineRenderer.endColor = c1;
        lineRenderer.startWidth = 0.05f * blastRadius/10.0f;
        lineRenderer.endWidth = 0.05f * blastRadius / 10.0f;
        lineRenderer.positionCount = numSegments + 1;
        lineRenderer.useWorldSpace = true;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        // lineRenderer.renderingLayerMask = (uint)LayerMask.NameToLayer("MainCamOnly");

        UpdateBlastPreviewRangeRenderer();
    }

    public void UpdateBlastPreviewRangeRenderer() {
        float deltaTheta = (float)(360.0f) / numSegments;

        Vector3 centerPt = transform.position;
        Vector3 swingArm = Camera.main.transform.up;
        Vector3 pivotVector = Camera.main.transform.forward;

        float theta = Random.Range(0.0f,1.0f) * deltaTheta; // randomize/stagger vertex offset

        for (int i = 0; i < numSegments + 1; i++)
        {
            Vector3 swingOff = Quaternion.AngleAxis(theta, pivotVector) * swingArm * blastRadius;
            Vector3 pos = centerPt + swingOff;
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
}
