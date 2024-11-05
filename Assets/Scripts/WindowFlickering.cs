using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowFlickering : MonoBehaviour
{
    [SerializeField] private List<Transform> windowPaneManager;
    [SerializeField] private int maxWindowsToChange;
    [SerializeField] private float windowChangeDelay;
    [SerializeField] private float maxTimerDelay;

    private List<GameObject> windowPanes;
    private int randomIndex;
    private float timer = 0.0f;

    private void Start()
    {
        windowPanes = new List<GameObject>();
        foreach (Transform windowParent in windowPaneManager)
        {
            foreach (Transform child in windowParent)
            {
                windowPanes.Add(child.gameObject);
            }
        }
        FlipWindowLights();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > windowChangeDelay)
        {
            FlipWindowLights();
            timer = 0.0f;
        }
    }

    private void FlipWindowLights()
    {
        for (int i = 0; i < maxWindowsToChange; ++i)
        {
            int change = Random.Range(0, 2);
            if (change != 0)
            {
                float delay = Random.Range(0.0f, maxTimerDelay);
                Invoke("FlipRandomWindowLight", delay);
            }
        }
    }

    private void FlipRandomWindowLight()
    {
        randomIndex = Random.Range(0, windowPanes.Count);
        GameObject chosenWindow = windowPanes[randomIndex];
        chosenWindow.SetActive(!chosenWindow.activeSelf);
    }
}
