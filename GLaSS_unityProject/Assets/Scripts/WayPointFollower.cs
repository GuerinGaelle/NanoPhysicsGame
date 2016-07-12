using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class WayPointFollower : MonoBehaviour {

    // Set in inspector
    public Transform MovingObject;
    public Transform WaypointContainer;

    public float TimeBetweenWayPoints = 1;

    private bool shouldMove = true;
    private List<Transform> wayPoints;
    private int currWayPoint = 0;

	void Start ()
    {
        // Initialize waypoints list
        wayPoints = new List<Transform>();
        for (int i = 0; i < WaypointContainer.childCount; i++)
        {
            wayPoints.Add(WaypointContainer.GetChild(i));
        }

        StartCoroutine("FollowPath");
    }

    void Update()
    {
        //Vector2 newPos = Vector2.Lerp(MovingObject.transform.position, wayPoints[currWayPoint].position, 0);
        //MovingObject.GetComponent<Rigidbody2D>().MovePosition(newPos);
    }

    void MoveToWayPoint(float time)
    {
        Vector2 newPos = Vector2.Lerp(MovingObject.transform.position, wayPoints[currWayPoint].position, time / TimeBetweenWayPoints);
        MovingObject.GetComponent<Rigidbody2D>().MovePosition(newPos);

        //MovingObject.GetComponent<Rigidbody2D>().MovePosition(point.position);
    }

    IEnumerator FollowPath()
    {
        int i = 0;
        float time = 0;

        while (shouldMove)
        {
            while(time <= TimeBetweenWayPoints)
            {
                time += Time.deltaTime;
                MoveToWayPoint(time);
                yield return new WaitForEndOfFrame();
            }

            if (i != wayPoints.Count - 1)
                i++;
            else
                i = 0;

            currWayPoint = i;
            time = 0;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}