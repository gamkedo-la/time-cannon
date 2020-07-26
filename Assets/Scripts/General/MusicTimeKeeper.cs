using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicTimeKeeper : MonoBehaviour
{
	public AudioMixer mixer;
	public float minTempoMult = 0.9f;
	public float maxTempoMult = 1.1f;

	private AudioMixerGroup[] musicGroup;
	private AudioMixerGroup[] sfxGroup;

	private void Start() {
		musicGroup = mixer.FindMatchingGroups("Music");
		sfxGroup = mixer.FindMatchingGroups("SFX");
	}

	private void Update() {
		float speedDampen = 1;
		float pitchDampen = 1;

		if (Input.GetKey(KeyCode.Space)) {
			speedDampen = 0.5f;
			pitchDampen = 2f;
		}

		mixer.SetFloat("MusicSpeedMult", Lerp(0f, minTempoMult, 2f, maxTempoMult, Mathf.Abs(TimeKeeper.instance.fakeTimePace)) * speedDampen);
		mixer.SetFloat("MusicPitchMult", Lerp(0f, maxTempoMult, 2f, minTempoMult, Mathf.Abs(TimeKeeper.instance.fakeTimePace)) * pitchDampen);
	}

	private float Lerp(float x1, float y1, float x2, float y2, float x) {
		return y1 + (x - x1) * ((y2 - y1) / (x2 - x1));
	}
}


/*
x1 = startTime
y1 = startVolume		

x2 = endTime
y2 = endVolume		

x = currentTime
y = y1 + (x - x1) * ((y2 - y1) / (x2 - x1))
y = y1 + ((x – x1) / (x2 – x1)) * (y2 – y1)
*/
