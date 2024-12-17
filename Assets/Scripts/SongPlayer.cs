using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> songClips;

    //In case need to use the original songs index
    private List<AudioClip> originIndex;
    private AudioSource audioSource;
    private int currentPlayIndex;
    void Start()
    {
        RandomizeList();
        currentPlayIndex = 0;
        audioSource = GetComponent<AudioSource>();
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

        currentPlayIndex += 1 % songClips.Count;
    }
}
