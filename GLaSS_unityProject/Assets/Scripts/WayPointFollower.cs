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

    void MoveToWayPoint(Rigidbody2D rigid, Vector3 oldPos, Vector3 newPos, float time)
    {
        Vector2 pos = Vector2.Lerp(oldPos, newPos, time / TimeBetweenWayPoints);
        rigid.MovePosition(pos);
    }

    IEnumerator FollowPath()
    {
        int _nbPoint = 0;
        float _time = 0;
        Rigidbody2D _rigid = MovingObject.GetComponent<Rigidbody2D>();

        while (shouldMove)
        {
            Vector3 _newPos = wayPoints[currWayPoint].position;
            Vector3 _oldPos = new Vector3();
            if (_nbPoint != 0)
                _oldPos = wayPoints[currWayPoint-1].position;
            else
                _oldPos = wayPoints[wayPoints.Count-1].position;

            while (_time <= TimeBetweenWayPoints)
            {
                _time += Time.deltaTime;
                MoveToWayPoint(_rigid, _oldPos, _newPos, _time);
                yield return new WaitForEndOfFrame();
            }

            if (_nbPoint != wayPoints.Count - 1)
                _nbPoint++;
            else
                _nbPoint = 0;

            currWayPoint = _nbPoint;
            _time = 0;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}