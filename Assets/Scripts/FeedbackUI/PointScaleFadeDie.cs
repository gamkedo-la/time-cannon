using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointScaleFadeDie : MonoBehaviour
{
    private float scaleMax = 1.4f;
    private float scaleNow = 1.0f;
    private float scaleRate = 0.1f;
    private float initialScale = 1.0f;
    private float randLateralDrift = 0.0f;
    private Text myLabel;

    void Awake()
    {
        initialScale = transform.localScale.x;
        myLabel = GetComponentInChildren<Text>();
    }

    public void SetText(string whichWords,float scaleAdjustment)
    {
        myLabel.text = whichWords;

        if(scaleAdjustment>1.0f)
        {
            scaleAdjustment -= 1.0f;
            scaleAdjustment *= 0.3f; // was getting too huge too fast, so dampening
            scaleAdjustment += 1.0f;
        }

        initialScale *= scaleAdjustment;
        scaleNow *= scaleAdjustment;
        scaleMax *= scaleAdjustment;
        randLateralDrift = Random.Range(-4.0f, 4.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * scaleMax * 0.5f +
                                transform.right * Time.deltaTime * randLateralDrift;
        scaleNow += Time.deltaTime * scaleRate;
        transform.localScale = Vector3.one * scaleNow * initialScale;

        Color fadeCol = myLabel.color;

        // holding fade firmer before falloff by squaring complement
        fadeCol.a = 1.0f-(scaleMax - scaleNow)/(scaleMax-1.0f);
        fadeCol.a *= fadeCol.a;
        fadeCol.a = 1.0f-fadeCol.a;

        myLabel.color = fadeCol;

        if (scaleNow > scaleMax)
        {
            Destroy(gameObject);
        }
    }
}
