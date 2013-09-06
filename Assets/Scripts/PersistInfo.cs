using UnityEngine;
using System.Collections;

/// <summary>
/// Component of Level1 scenes PersistentObj gameobject
/// </summary>
public class PersistInfo : MonoBehaviour
{
	public AudioClip[] MusicTracks;
	
	private int currentTrack = 0;
	
	void Awake()
	{
		var copies = GameObject.FindGameObjectsWithTag("Persistent").Length;
		if (copies > 1) //keep only the first copy
		{
			Destroy(gameObject);
		}
		else //preserve this gameobject even when loading another scene
		{
			DontDestroyOnLoad(gameObject);
		}
	}
	
	void Start()
	{
	}
	
	void Update()
	{
		if (audio.isPlaying == false && Globals.IsSoundOn)
		{
			playNextMusicTrack();
		}
	}
	
	private void playNextMusicTrack()
	{
		//set the track
		if (currentTrack == MusicTracks.Length - 1)
		{
			currentTrack = 0;
		}
		else
		{
			currentTrack++;
		}
		
		//play it
		audio.clip = this.MusicTracks[this.currentTrack];
		audio.Play();
	}
}