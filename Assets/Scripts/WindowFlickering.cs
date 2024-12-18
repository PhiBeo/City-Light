using System.Collections.Generic;
using UnityEngine;

public class WindowFlickering : MonoBehaviour
{
    [SerializeField] private List<Transform> windowPaneManager;
    [SerializeField] private int maxWindowsToChange;
    [SerializeField] private float windowChangeDelay;
    [SerializeField] private float maxTimerDelay;

    private List<GameObject> windowPanes;
    private List<GameObject> windowPanesOff; //Originally was changing the color and opacity of the material on the window, but this caused excessive lag in VR, this was the least laggy solution
    private int randomIndex;
    private float timer = 0f;

    private GameObject chosenWindowOn;
    private GameObject chosenWindowOff;

    private float musicStartTime = 13f; //Manually tracked start time of song
    private float musicSlowWindowDelay = 6f; //slow start of song until 60 seconds, and again at 240 seconds
    private float musicFastWindowDelay = 3f; //faster middle
    private float musicWindowDelay = 3f;
    private float playTimer = 0f;

    private int musicFlipCount = 0;

    private bool firstplay = true;

    private void Start()
    {
        windowPanes = new List<GameObject>();
        windowPanesOff = new List<GameObject>();
        foreach (Transform windowParent in windowPaneManager) //Fills the window lists on start for later use
        {
            int i = 0;
            foreach (Transform windowList in windowParent)
            {
                if (i == 0)
                {
                    foreach (Transform window in windowList)
                    {
                        windowPanes.Add(window.gameObject);
                    }
                    i = 1;
                }
                else
                {
                    foreach (Transform window in windowList)
                    {
                        windowPanesOff.Add(window.gameObject);
                    }
                }
            }
        }

        for (int i = 0; i < windowPanes.Count; ++i) //Starts the scene with random windows off
        {
            if (RandomBool(2))
            {
                windowPanes[i].SetActive(false);
                windowPanesOff[i].SetActive(true);
            }
        }
    }

    void Update()
    {
        float dt = Time.deltaTime;
        timer += dt;
        playTimer += dt;

        if (playTimer < musicStartTime) //start time of song
        {
            return;
        }

        if (musicFlipCount >= 3) //Three piano notes before longer break
        {
            musicWindowDelay = musicSlowWindowDelay;
        }
        else
        {
            musicWindowDelay = musicFastWindowDelay;
        }

        if (playTimer < 60f || playTimer > 240f) //single window flip
        {
            if (timer > musicWindowDelay) //Flips windows after the appropriate time to sync with music, handling delays between notes
            {
                FlipWindowLights(maxWindowsToChange, false);
                timer = 0.0f;
                if (musicFlipCount >= 3)
                {
                    musicFlipCount = 0;
                }
                musicFlipCount++;
            }
        }
        else //triplet window flip
        {
            if (timer > musicWindowDelay)
            {
                FlipWindowLights(maxWindowsToChange, true);
                timer = 0.0f;
                if (musicFlipCount >= 3)
                {
                    musicFlipCount = 0;
                }
                musicFlipCount++;
            }
        }
    }

    private void FlipWindowLights(int toChange, bool triplet)
    {
        for (int i = 0; i < toChange; ++i)
        {
            if (!triplet)
            {
                FlipRandomWindowLight();
            }
            else
            {
                FlipRandomWindowLight(); //Flip lights three times with slight delay to 'trill' the lights with the piano
                Invoke("FlipRandomWindowLight", 0.1f);
                Invoke("FlipRandomWindowLight", 0.2f);
            }
        }
    }

    private void FlipRandomWindowLight() 
    {
        randomIndex = Random.Range(0, windowPanes.Count); //Chooses a random index to flip, saves time from cycling through entire list of windows
        chosenWindowOn = windowPanes[randomIndex];
        chosenWindowOff = windowPanesOff[randomIndex];
        chosenWindowOn.SetActive(!chosenWindowOn.activeSelf); //Flips window to the opposite state
        chosenWindowOff.SetActive(!chosenWindowOff.activeSelf); //Flips matching off window to the opposite state

    }

    private bool RandomBool(int max)
    {
        int change = Random.Range(0, max);
        if (change != 0)
        {
            return true;
        }
        return false;
    }

    public void StartTimer()
    {
        if (firstplay)
        {
            firstplay = false;
            return;
        }
        timer = 0;
        playTimer = 0f;
    }
}
