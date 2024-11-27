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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timer = Random.Range(minInterval, maxInterval);
        startPos = transform.position;
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

        while(randIndex == lastPlayedSound) 
        {
            randIndex = Random.Range(0, audioClips.Count);
        }

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
}
