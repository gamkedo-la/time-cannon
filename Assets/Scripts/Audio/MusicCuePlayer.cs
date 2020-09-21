using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCuePlayer : MonoBehaviour
{
	public GameObject musicPrefab;
	private AudioSource currentSource;
	private float outTime = 1f;

	public List<AudioClip_Dir_Class> musicPlaylist;
	private Queue<AudioClip_Dir_Class> musicQueue = new Queue<AudioClip_Dir_Class>();

	public bool playOnAwake = true;
	bool playing = false;

	void Start()
	{
		for (int i = 0; i < musicPlaylist.Count; i++)
		{
			musicQueue.Enqueue(musicPlaylist[i]);
		}

		StartCoroutine(LateStart());
	}

	void OnDestroy()
	{
		StopAllCoroutines();
	}

	public void AddTrack(AudioClip track, float duration)
	{
		AudioClip_Dir_Class newTrack = new AudioClip_Dir_Class(track, duration);
		musicQueue.Enqueue(newTrack);
	}

	public void AddTrack(AudioClip_Dir_Class newTrack)
	{
		musicQueue.Enqueue(newTrack);
	}

	public void Play()
	{
		if(!playing) StartTrack();
	}

	public void Stop()
	{
		currentSource.Pause();
		playing = false;
	}

	private void StartTrack()
	{
		AudioClip_Dir_Class nextTrack;

		if (musicQueue.Count > 1) nextTrack = musicQueue.Dequeue();
		else nextTrack = musicQueue.Peek();

		currentSource = Instantiate(musicPrefab, gameObject.transform).GetComponent<AudioSource>();

		currentSource.clip = nextTrack.track;
		outTime = nextTrack.duration;

#if !UNITY_WEBGL
		if (TimeKeeper.instance.fakeTimePace < 0f) currentSource.time = outTime;
#endif

		currentSource.Play();
		playing = true;
	}

	IEnumerator LoopTracks()
	{
		while (true)
		{
			if (playing)
			{
				if (TimeKeeper.instance.fakeTimePace > 0 && currentSource.time >= outTime) StartTrack();
#if !UNITY_WEBGL
				if (TimeKeeper.instance.fakeTimePace < 0 && currentSource.time <= 0f) StartTrack();
#endif
			}

			yield return null;
		}
	}

	IEnumerator LateStart()
	{
		yield return new WaitForEndOfFrame();
		
		StartCoroutine(LoopTracks());
		if (playOnAwake) Play();
	}
}

[System.Serializable]
public class AudioClip_Dir_Class
{
	public AudioClip track;
	public float duration;

	public AudioClip_Dir_Class(AudioClip clip, float outTime)
	{
		track = clip;
		duration = outTime;
	}
}