using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class GUI_Typewriter : MonoBehaviour {
 
    public Text animateText;
    public float holdTime = 4.0f;
    public float fadeTime = 2.0f;
    private float age = 0.0f;
    private string fullText;
   
    private int frameCount = 0;

    public void Start() {
        frameCount = 0;
    }

    public void Update()
    {
        if (!animateText) return;

        // allow the intro text changer at least 
        // one frame to update string to the right time-period
        frameCount++;
        if (frameCount<5) return; 
        if (frameCount==5) {
            // init typewriter effect
            age = 0f;
            fullText = animateText.text;
            Debug.Log("TYPEWRITER will type out: "+fullText);
        }
        
        if (age>holdTime+fadeTime) {
            // Debug.Log("Typewriter complete!");
            gameObject.active = false; // go away forever
            return;
        }
        
        age +=  Time.deltaTime;
        float percent = 1.0f;

        if (age<holdTime) {
            percent = age/holdTime;
            if (percent<0) percent = 0;
            if (percent>1) percent = 1;

            int len = (int)(fullText.Length*percent);
            string temp = fullText.Substring(0,len+1);
            // Debug.Log("TYPEWRITER"+len+": "+temp);
            animateText.text = temp;

        }

        if (age>holdTime) {
            
            percent = (age-holdTime)/fadeTime;
            if (percent<0) percent = 0;
            if (percent>1) percent = 1;

            // Debug.Log("Typewriter percent:"+percent+" age:"+age);

            animateText.color = new Color(
                animateText.color.r, 
                animateText.color.g, 
                animateText.color.b, 
                1 - percent);
        }

    }

}