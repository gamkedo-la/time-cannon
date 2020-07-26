using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicTimeKeeper : MonoBehaviour
{
	public AudioMixer mixer;
	public float tempoMultRange = 0.2f;
	public List<AudioSource> sources;

	private bool hasSources = false;
	private float minTempoMult;
	private float maxTempoMult;
	private List<AudioSource> deadSources = new List<AudioSource>(0);

	private void Start() {
		if (sources.Count >= 1) {
			hasSources = true;
		}

		minTempoMult = 1 - tempoMultRange;
		maxTempoMult = 1 + tempoMultRange;
	}

	private void Update() {
		float speedDampen = 1;
		float pitchDampen = 1;

		if (Input.GetKey(KeyCode.Space)) {
			speedDampen = 0.5f;
			pitchDampen = 2f;
		}

		if (hasSources) {
			if (TimeKeeper.instance.fakeTimePace < 0) speedDampen *= -1;
			foreach (AudioSource source in sources) {
				if (source == null) {
					deadSources.Add(source);
					continue;
				}
				source.pitch = Lerp(0f, minTempoMult, 2f, maxTempoMult, Mathf.Abs(TimeKeeper.instance.fakeTimePace)) * speedDampen;
			}
		} else {
			mixer.SetFloat("MusicSpeedMult", Lerp(0f, minTempoMult, 2f, maxTempoMult, Mathf.Abs(TimeKeeper.instance.fakeTimePace)) * speedDampen);
		}
		mixer.SetFloat("MusicPitchMult", Lerp(0f, maxTempoMult, 2f, minTempoMult, Mathf.Abs(TimeKeeper.instance.fakeTimePace)) * pitchDampen);

		if (deadSources.Count >= 1) {
			foreach (AudioSource source in deadSources) {
				sources.Remove(source);
			}
			deadSources.Clear();
			if (sources.Count == 0) hasSources = false;
		}
	}

	public void AddSource(AudioSource newSource) {
		sources.Add(newSource);
		hasSources = true;
	}

	public void RemoveSource(AudioSource oldSource) {
		sources.Remove(oldSource);
		if (sources.Count == 0) hasSources = false;
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
