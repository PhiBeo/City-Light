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
    private List<Color> paneColorList;
    private int randomIndex;
    private float timer = 0.0f;

    private Color transparent = new Color(255, 255, 255, 0);
    private Color opaque = new Color(0, 0, 0, 255);

    bool firstPass = false;

    private string emissionColor = "_EmissionColor";

    private void Start()
    {
        windowPanes = new List<GameObject>();
        paneColorList = new List<Color>();
        foreach (Transform windowParent in windowPaneManager)
        {
            foreach (Transform child in windowParent)
            {
                windowPanes.Add(child.gameObject);
                Material mat;
                if (!HasRenderer(child.gameObject))
                {
                    continue;
                }
                mat = child.GetComponent<Renderer>().material;
                paneColorList.Add(mat.GetColor(emissionColor));
            }
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
        Material mat = chosenWindow.GetComponent<Renderer>().material;
        if (mat.GetColor(emissionColor) == paneColorList[randomIndex])
        {
            mat.SetColor(emissionColor, Color.black);
            mat.SetColor("_Color", opaque);
            return;
        }
        mat.SetColor(emissionColor, paneColorList[randomIndex]);
        mat.SetColor("_Color", transparent);

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
