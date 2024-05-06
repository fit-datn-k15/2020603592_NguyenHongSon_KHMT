using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCarWaypoint : MonoBehaviour
{
    [Header("Opponent Car")]
    [SerializeField] private OpponentCar opponentCar;
    [SerializeField] private Waypoint currentWaypoint;

    private void Start()
    {
        opponentCar.LocateDestination(currentWaypoint.getPosition());
    }

    private void Update()
    {
        if (opponentCar.IsDestinationReached())
        {
            currentWaypoint = currentWaypoint.getNextWaypoint();
            opponentCar.LocateDestination(currentWaypoint.getPosition());
        }
    }
}
