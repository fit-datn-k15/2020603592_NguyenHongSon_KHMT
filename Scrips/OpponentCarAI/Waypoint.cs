using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{


    [Header("Waypoint Status")]
    //[SerializeField] private Waypoint previousWaypoint;
    //[SerializeField] private Waypoint nextWaypoint;
    [SerializeField] public Waypoint previousWaypoint;
    [SerializeField] public Waypoint nextWaypoint;


    [Range(0f, 5f)][SerializeField] private float waypointWith = 5f;
    // Start is called before the first frame update


    public Vector3 getPosition()
    {
        Vector3 minBound = transform.position + transform.right * waypointWith / 2f;
        Vector3 maxBound = transform.position - transform.right * waypointWith / 2f;
        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));

    }
    public Waypoint getpreviousWaypoint()
    { return previousWaypoint; }
    public Waypoint getNextWaypoint() 
    { return nextWaypoint; }


    public void SettpreviousWaypoint(Waypoint waypoint)
    { this.previousWaypoint = waypoint; }
    public void SetNextWaypoint(Waypoint waypoint)
    { this.nextWaypoint = waypoint; }

    public float GetWaypointWith()
    {
        return waypointWith;
    }
}