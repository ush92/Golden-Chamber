using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject platform;

    public float moveSpeed;

    private Transform currentPoint;
    public Transform[] points;

    public int pointSelection;

    public bool isTurnedOn = true;

    void Start()
    {
        currentPoint = points[pointSelection];
    }

    void Update()
    {
        if(isTurnedOn)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);

            if (platform.transform.position == currentPoint.position)
            {
                pointSelection++;
                if (pointSelection >= points.Length)
                {
                    pointSelection = 0;
                }

                currentPoint = points[pointSelection];
            }
        }
    }
}
