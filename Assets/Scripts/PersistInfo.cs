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
        if (GetComponent<AudioSource>().isPlaying == false && Globals.IsSoundOn)
        {
            playNextMusicTrack();
        }
    }

    private void playNextMusicTrack()
    {
        if (this.MusicTracks.Length == 0)
        {
            Debug.Log("No music. Add music to UiInGame(2D)'s PersistInfo script");
            return;
        }

        //set the track
        else if (currentTrack == MusicTracks.Length - 1)
        {
            currentTrack = 0;
        }
        else
        {
            currentTrack++;
        }

        //play it
        GetComponent<AudioSource>().clip = this.MusicTracks[this.currentTrack];
        GetComponent<AudioSource>().Play();
    }
}