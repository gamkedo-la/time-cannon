using UnityEngine;
using System.Collections;

public class OceanScrollScript : MonoBehaviour {
	
	public float xRate = 0.0f;
	public float yRate = 1.0f;

	public float xFixedOffset = 0.0f;
	public float yFixedOffset = 0.0f;

    private float scrollSpeed = 0.05f;
    private float offset = 0.0f;

    public Material scrollMat;

    void OnApplicationQuit() {
        scrollMat.mainTextureOffset = Vector2.zero;
        scrollMat.SetTextureOffset("_DetailAlbedoMap", Vector2.zero);
    }

    void Update () {
       float rate = scrollSpeed * TimeKeeper.instance.fakeTimeDelta;

       offset += rate;
       scrollMat.mainTextureOffset = new Vector2 (xRate * offset + xFixedOffset, yRate * offset + yFixedOffset);
        scrollMat.SetTextureOffset("_DetailAlbedoMap", new Vector2(-1.1f*xRate * offset + xFixedOffset, -0.3f * yRate * offset + yFixedOffset));
    }
}