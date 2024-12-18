using System.Collections.Generic;
using UnityEngine;

public class SurroundSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField] private float minInterval = 5.0f;
    [SerializeField] private float maxInterval = 15.0f;

    [SerializeField] private bool randomLocation = true;
    [SerializeField] private float maxDistance = 100;

    private AudioSource audioSource;
    private float timer = 0;

    private int lastPlayedSound = -1;
    private Vector3 startPos;

    private Dictionary<int, int> playedTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timer = Time.time + Random.Range(minInterval, maxInterval);
        startPos = transform.position;

        playedTime = new Dictionary<int, int>();

        for(int i = 0; i < audioClips.Count; i++)
        {
            playedTime.Add(i, 0);
        }
    }

    void Update()
    {
        if (audioSource.isPlaying) return;
        
        if(Time.time >= timer)
        {
            if(randomLocation)
            {
                MoveToRandomLocation();
            }

            PlayRandomSound();
        }
    }

    void PlayRandomSound()
    {
        int randIndex = Random.Range(0, audioClips.Count);

        while(randIndex == lastPlayedSound) 
        {
            randIndex = Random.Range(0, audioClips.Count);
        }

        randIndex = SoundRepeatCheck(randIndex);

        audioSource.clip = audioClips[randIndex];
        audioSource.Play();

        lastPlayedSound = randIndex;

        playedTime[randIndex]++;

        timer = Time.time + Random.Range(minInterval, maxInterval);
    }

    void MoveToRandomLocation()
    {
        float xPos = Random.Range(startPos.x - maxDistance, startPos.x + maxDistance);
        float zPos = Random.Range(startPos.z - maxDistance, startPos.z + maxDistance);

        transform.position = new Vector3(xPos, transform.position.y, zPos);
    }

    int SoundRepeatCheck(int value)
    {
        int averagePlayTime = 0;

        for(int i = 0; i < playedTime.Count; i++)
        {
            averagePlayTime += playedTime[i];
        }

        averagePlayTime = averagePlayTime / playedTime.Count;

        //Play the random sound
        if(playedTime[value] <= averagePlayTime + 1)
        {
            return value;
        }

        //if random sound is played too many time
        //selected the smallest played sound
        int smallestIndex = -1;
        int smallestValue = 999;

        for(int i = 0; i < playedTime.Count; i++)
        {
            if(playedTime[i] < smallestValue)
            {
                smallestIndex = i;
                smallestValue = playedTime[i];
            }
        }

        return smallestIndex;
    }
}
