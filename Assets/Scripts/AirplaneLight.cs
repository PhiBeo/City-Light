using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneLight : MonoBehaviour
{
    [SerializeField] private GameObject[] lights = new GameObject[2];

    [SerializeField] private float flickTime = 2f;

    private float lastFlickTime = 0;

    void Update()
    {
        if(Time.time > lastFlickTime + flickTime)
        {
            LightToggle();
            lastFlickTime = Time.time;
        }
    }

    void LightToggle()
    {
        for(int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(!lights[i].activeSelf);
        }
    }
}
