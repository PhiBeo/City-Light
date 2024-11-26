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
    private List<int> soundPlayedCount;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timer = Random.Range(minInterval, maxInterval);
        startPos = transform.position;

        soundPlayedCount = new List<int>();

        for(int i = 0; i < audioClips.Count; i++)
        {
            soundPlayedCount.Add(0);
        }
    }

    void Update()
    {
        if (audioSource.isPlaying) return;
        
        timer -= Time.deltaTime;
        if(timer <= 0f)
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

        randIndex = SoundWeightCheck(randIndex);

        soundPlayedCount[randIndex]++;
        audioSource.clip = audioClips[randIndex];
        audioSource.Play();

        lastPlayedSound = randIndex;

        timer = Random.Range(minInterval, maxInterval);
    }

    void MoveToRandomLocation()
    {
        float xPos = Random.Range(startPos.x - maxDistance, startPos.x + maxDistance);
        float zPos = Random.Range(startPos.z - maxDistance, startPos.z + maxDistance);

        transform.position = new Vector3(xPos, transform.position.y, zPos);
    }

    //mainly use for small size list
    int SoundWeightCheck(int index)
    {
        int average = 0;
        int total = 0;

        for (int i = 0; i < audioClips.Count; i++)
        {
            total += soundPlayedCount[i];
        }

        average = total / soundPlayedCount.Count;

        if(soundPlayedCount[index] <= average)
        {
            Debug.Log("Index is weight is normal");
            return index;
        }

        Debug.Log("Rebalance Weight");

        int smallestPlayIndex = 0;
        int smallestplayedTime = 99;

        for(int i = 0; i < audioClips.Count; i++)
        {
            if(soundPlayedCount[i] < smallestplayedTime)
            {
                smallestplayedTime = soundPlayedCount[i];
                smallestPlayIndex = i;
            }
        }

        return smallestPlayIndex;
    }
}
