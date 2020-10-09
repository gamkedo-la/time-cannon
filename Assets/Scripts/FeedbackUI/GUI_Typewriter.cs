using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class GUI_Typewriter : MonoBehaviour {
 
    public Text animateText;
    public float holdTime = 4.0f;
    public float fadeTime = 2.0f;
    private float age = 0.0f;
    private string fullText;
   
    public void Start() {
        fullText = animateText.text;
        Debug.Log("TYPEWRITER will type out: "+fullText);
    }

    public void Update()
    {
        if (age>holdTime+fadeTime) {
            Debug.Log("Typewriter complete!");
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
            Debug.Log("TYPEWRITER"+len+": "+temp);
            animateText.text = temp;

        }

        if (age>holdTime) {
            
            percent = (age-holdTime)/fadeTime;
            if (percent<0) percent = 0;
            if (percent>1) percent = 1;

            Debug.Log("Typewriter percent:"+percent+" age:"+age);

            animateText.color = new Color(
                animateText.color.r, 
                animateText.color.g, 
                animateText.color.b, 
                1 - percent);
        }

    }

}