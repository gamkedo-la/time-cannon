using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructOnAudioClipNotPLaying : MonoBehaviour
{
	private AudioSource theClip;

    void Start()
    {
		theClip = GetComponent<AudioSource>();
    }
	
    void Update()
    {
        if (!theClip.isPlaying)
		{
			Destroy(gameObject, 1f);
		}
    }
}
