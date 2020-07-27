using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicComponent : MonoBehaviour {
	private AudioSource[] sources;
	private MusicTimeKeeper keeper;

#if !UNITY_WEBGL
	private void Start() {
		sources = gameObject.GetComponentsInChildren<AudioSource>();
		keeper = FindObjectOfType<MusicTimeKeeper>();
		
		foreach (AudioSource source in sources) {
			source.outputAudioMixerGroup = keeper.group;
			keeper.AddSource(source);
		}
	}

	private void OnDestroy() {
		foreach (AudioSource source in sources) {
			keeper.RemoveSource(source);
		}
	}
#endif
}
