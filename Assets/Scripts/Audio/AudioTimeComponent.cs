using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTimeComponent : MonoBehaviour
{
	public bool reversable = true;
	private AudioSource[] sources;

	private void Start() {
		sources = gameObject.GetComponentsInChildren<AudioSource>();
	}

	private void Update() {
		foreach (AudioSource source in sources) {
			if (reversable)
			{
				source.pitch = TimeKeeper.instance.fakeTimePace;
			}
			else
			{
				source.pitch = Mathf.Abs(TimeKeeper.instance.fakeTimePace);
			}

			if (Input.GetKey(KeyCode.Space)) {
				source.pitch *= 0.4f;
			}
		}
	}
}
