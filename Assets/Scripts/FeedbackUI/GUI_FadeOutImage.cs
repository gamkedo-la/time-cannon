using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class GUI_FadeOutImage : MonoBehaviour {
 
    public RawImage fadeOutThisRawImage;
    public float holdTime = 2.0f;
    public float fadeTime = 2.0f;
    private float age = 0.0f;
   
    public void Update()
    {
        if (age>holdTime+fadeTime) {
            Debug.Log("Fade complete!");
            gameObject.active = false; // go away forever
            return;
        }
        
        age +=  Time.deltaTime;

        if (age>holdTime) {
            
            float percent = (age-holdTime)/fadeTime;
            if (percent<0) percent = 0;
            if (percent>1) percent = 1;

            Debug.Log("Fade percent:"+percent+" age:"+age);

            fadeOutThisRawImage.color = new Color(
                fadeOutThisRawImage.color.r, 
                fadeOutThisRawImage.color.g, 
                fadeOutThisRawImage.color.b, 
                1 - percent);
        }

    }

}