using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private RoadSearcher _searcher;
    private static float CLOSE_DISTANCE = 0.01f;
    private Vector3 _currentDestination;
    private Road _currentRoad;
    private bool _hasRoad = false;

    private void FixedUpdate()
    {
        if (_hasRoad)
        {
            if (Vector3.Distance(transform.position, _currentDestination) < CLOSE_DISTANCE)
            {
                if (_currentRoad.WaypointsWorld.TryDequeue(out Vector3 waypoint))
                {
                    _currentDestination = waypoint;
                }
                else
                {
                    _hasRoad = false;
                    Debug.Log("Reached the end of the road");
                    return;
                }
            }
            Move(_currentDestination);
        }
    }

    private void OnDrawGizmos()
    {
        if (_currentRoad == null) return;
        foreach (Vector3 point in _currentRoad.WaypointsWorld)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
    }

    public void Walk(Room room)
    {
        Road road = _searcher.FindRoadFromTo(transform.position, room.transform.position);
        if (road == null || road.WaypointsWorld.Count < 2)
        {
            _hasRoad = false;
            Debug.Log("Road did not find. Can't move to " + room.Name);
        }
        else
        {
            _currentRoad = new Road(road);
            _currentDestination = _currentRoad.WaypointsWorld.Dequeue();
            _hasRoad = true;
            Debug.Log("Road found. Moving to " + room.Name);
        }
    }

    private void Move(Vector3 pos)
    {
        transform.parent.transform.position = OneStepForwardTo(pos);
    }

    private Vector3 OneStepForwardTo(Vector3 pos)
    {
        return Vector3.MoveTowards(transform.position, pos, _speed * Time.fixedDeltaTime);
    }
}
