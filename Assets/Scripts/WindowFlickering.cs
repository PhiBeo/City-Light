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
    private List<GameObject> windowPanesOff;
    private int randomIndex;
    private float timer = 0.0f;

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
                }
                else
                {
                    foreach (Transform window in windowList)
                    {
                        windowPanesOff.Add(window.gameObject);
                    }
                }
            }
            i = 1;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > windowChangeDelay)
        {
            FlipWindowLights(maxWindowsToChange);
            timer = 0.0f;
        }
    }

    private void FlipWindowLights(int toChange)
    {
        for (int i = 0; i < toChange; ++i)
        {
            if (RandomBool(2))
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

    private bool HasRenderer(GameObject obj)
    {
        return obj.GetComponent<Renderer>() != null;
    }
}
