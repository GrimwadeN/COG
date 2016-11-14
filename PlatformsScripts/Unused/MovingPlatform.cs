using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    enum ELEVATORSTATE
    {
        MOVING,
        WAITING
    }

    public Transform[] waypoints;
    public float moveSpeed = 5;
    public float waitPeriod = 2;

    private int currentWaypoint = 1;
    private ELEVATORSTATE elevatorState;
    private float waitTimer;
    

	// Use this for initialization
	void Start () {
        elevatorState = ELEVATORSTATE.WAITING;
	}
	
	// Update is called once per frame
	void Update () {

        switch(elevatorState)
        {
            case ELEVATORSTATE.WAITING:
   
                /// <summary
                /// BAH HUMBUG I HATE THIS CODE
                /// </summary

                waitTimer += Time.deltaTime;
                // Wait on point for X seconds
                if (waitTimer >= 2)
                {
                    elevatorState = ELEVATORSTATE.MOVING;
                }
                
                break;

            case ELEVATORSTATE.MOVING:                
                // Swap the waypoint
                if ((waypoints[currentWaypoint].transform.position - transform.position).magnitude < 1)
                {
                    if (currentWaypoint == 0)
                    {
                        currentWaypoint++;
                    }
                    else
                    {
                        currentWaypoint--;
                    }
                }
                // if within distance of waypoint, pause so player cana get on or off
                if ((waypoints[currentWaypoint].transform.position - transform.position).magnitude < 1.1f)
                {
                    waitTimer = 0;
                    elevatorState = ELEVATORSTATE.WAITING;
                }

                // Move to next waypoint
                transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, moveSpeed * Time.deltaTime);

                break;
        }

        
    }
}
