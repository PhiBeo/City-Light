using UnityEngine;

public class AirplaneMove : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [Tooltip("Time to move between two point")]
    [SerializeField] private float moveSpeed = 5f;

    [Tooltip("Set the time that object will start moving after scene started in seconds")]
    [SerializeField] private float startTime;

    private float travelDist;
    private float distCover = 0;
    private float fracOfDist = 0;

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

        distCover = (Time.time - startTime) * moveSpeed;

        fracOfDist = distCover / travelDist;

        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, fracOfDist);

        transform.rotation.SetLookRotation(endPoint.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPoint.position, endPoint.position);
    }
}
