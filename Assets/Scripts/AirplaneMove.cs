using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct PlaneRoute
{
    public Transform startPoint;
    public Transform endPoint;
    public float distance;
}


public class AirplaneMove : MonoBehaviour
{
    [SerializeField] private PlaneRoute[] routes;

    [Tooltip("Time to move between two point")]
    [SerializeField] private float moveSpeed = 5f;

    [Tooltip("Set the time that object will start moving after scene started in seconds")]
    [SerializeField] private float startTime;

    [SerializeField] private float minInterval = 3f;
    [SerializeField] private float maxInterval = 5f;

    private float distCover = 0;
    private float fracOfDist = 0;
    private int currentRoute = -1;
    private bool hasFlight = false;

    void Start()
    {
        for(int i = 0; i < routes.Length; i++)
        {
            routes[i].distance = Vector3.Distance(routes[i].startPoint.position, routes[i].endPoint.position);
        }
    }

    void Update()
    {
        if(Time.time > startTime && !hasFlight)
        {
            currentRoute = Random.Range(0, routes.Length);
            hasFlight = true;
        }

        if (!hasFlight) return;
        MoveObject();
        GenerateNewRounte();
    }

    void MoveObject()
    {
        distCover = (Time.time - startTime) * moveSpeed;

        fracOfDist = distCover / routes[currentRoute].distance;

        transform.position = Vector3.Lerp(routes[currentRoute].startPoint.position, routes[currentRoute].endPoint.position, fracOfDist);

        transform.rotation.SetLookRotation(routes[currentRoute].endPoint.position);
    }

    void GenerateNewRounte()
    {
        if (transform.position != routes[currentRoute].endPoint.position) return;

        hasFlight = false;
        startTime = Time.time + Random.Range(minInterval, maxInterval);
        currentRoute = Random.Range(0, routes.Length);
        Debug.Log("New route generate");
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < routes.Length; i++)
        {
            Gizmos.DrawLine(routes[i].startPoint.position, routes[i].endPoint.position);
        }
    }
}
