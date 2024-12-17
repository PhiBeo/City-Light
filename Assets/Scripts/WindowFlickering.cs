using System.Collections.Generic;
using UnityEngine;

public class WindowFlickering : MonoBehaviour
{
    [SerializeField] private List<Transform> windowPaneManager;
    [SerializeField] private int maxWindowsToChange;
    [SerializeField] private float windowChangeDelay;
    [SerializeField] private float maxTimerDelay;

    private List<GameObject> windowPanes;
    private List<GameObject> windowPanesOff;
    private int randomIndex;
    private float timer = 0f;

    private float musicStartTime = 13f;
    private float musicSlowWindowDelay = 6f; //slow start of song until 60 seconds, and again at 240 seconds
    private float musicFastWindowDelay = 3f; //faster middle
    private float musicWindowDelay = 3f;
    private float playTimer = 0f;

    private int musicFlipCount = 0;

    private void Start()
    {
        windowPanes = new List<GameObject>();
        windowPanesOff = new List<GameObject>();
        foreach (Transform windowParent in windowPaneManager)
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

        for (int i = 0; i < windowPanes.Count; ++i)
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

        if (playTimer < 13f) //start time of song
        {
            return;
        }

        if (musicFlipCount >= 3)
        {
            musicWindowDelay = 6f;
        }
        else
        {
            musicWindowDelay = 3f;
        }

        if (playTimer < 60f || playTimer > 240f) //single window flip
        {
            if (timer > musicWindowDelay)
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
            //if (RandomBool(2))
            //{
            //    float delay = Random.Range(0.0f, maxTimerDelay);
            //    //Invoke("FlipRandomWindowLight", delay);
            //    FlipRandomWindowLight();
            //}
            if (!triplet)
            {
                FlipRandomWindowLight();
            }
            else
            {
                FlipRandomWindowLight();
                Invoke("FlipRandomWindowLight", 0.1f);
                Invoke("FlipRandomWindowLight", 0.2f);
            }
            //float delay = Random.Range(0.0f, maxTimerDelay);
            //Invoke("FlipRandomWindowLight", delay);

        }
    }

    private void FlipRandomWindowLight()
    {
        randomIndex = Random.Range(0, windowPanes.Count);
        GameObject chosenWindow = windowPanes[randomIndex];
        chosenWindow.SetActive(!chosenWindow.activeSelf);
        windowPanesOff[randomIndex].SetActive(!chosenWindow.activeSelf);

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
}
