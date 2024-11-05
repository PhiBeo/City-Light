using UnityEngine;

public class AirplaneMove : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [Tooltip("Time to move between two point")]
    [SerializeField] private float moveTime = 5f;

    [Tooltip("Set the time that object will start moving after scene started in seconds")]
    [SerializeField] private float startTime;

    private float travelDist;

    void Start()
    {
        travelDist = Vector3.Distance(startPoint.position, endPoint.position);
    }

    void Update()
    {
        MoveObject();
    }

    void MoveObject()
    {
        if (Time.time <= startTime) return;

        float distCover = (Time.time - startTime) * moveTime;

        float fracOfDist = distCover / travelDist;

        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, fracOfDist);
    }
}
