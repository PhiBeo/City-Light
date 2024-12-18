using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> songClips;
    [SerializeField] private WindowFlickering windows;

    //In case need to use the original songs index
    private List<AudioClip> originIndex;
    private AudioSource audioSource;
    private int currentPlayIndex;
    void Start()
    {
        RandomizeList();
        currentPlayIndex = 0;
        audioSource = GetComponent<AudioSource>();

        //in case not set WindowFlickering in the inspector
        if(windows == null)
        {
            windows = FindObjectOfType<WindowFlickering>();
        }
    }

    void Update()
    {
        PlayNextSong();
    }

    void RandomizeList()
    {
        originIndex = songClips;

        System.Random rng = new System.Random();

        int n = songClips.Count;

        //shuffle the song list
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var value = songClips[k];
            songClips[k] = songClips[n];
            songClips[n] = value;
        }
    }

    void PlayNextSong()
    {
        if (audioSource.isPlaying) return;

        audioSource.clip = songClips[currentPlayIndex];

        audioSource.Play();

        windows.StartTimer();

        currentPlayIndex += 1 % songClips.Count;
    }
}
