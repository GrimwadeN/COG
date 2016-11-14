using UnityEngine;
using System.Collections;

public class ElevatorSwitch : MonoBehaviour {

    enum ELEVATORSTATE
    {
        IDLE,
        MOVING   
    }

    public GameObject elevator;
    [Tooltip("First element should be low point, second point should be highest position elevator goes to")]
    public Transform[] waypoint;
    [Tooltip("Elevator move speed")]
    public float speed = 5;

    private ELEVATORSTATE state;
    private int currentWaypoint = 0;
    private float minDistance = 0.2f;

	// Use this for initialization
	void Start () {
        state = ELEVATORSTATE.IDLE;
	}
	
	// Update is called once per frame
	void Update () {

        switch(state)
        {
            case ELEVATORSTATE.IDLE:
                // do nothing
                break;

            case ELEVATORSTATE.MOVING:
                elevator.transform.position = Vector3.Lerp(elevator.transform.position, waypoint[currentWaypoint].transform.position, speed * Time.deltaTime);
                if((elevator.transform.position - waypoint[currentWaypoint].transform.position).magnitude < minDistance)
                {
                    state = ELEVATORSTATE.IDLE;
                }
                break;
        }
	
	}

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.V))
            {
                if(currentWaypoint == 0)
                {
                    currentWaypoint++;
                    state = ELEVATORSTATE.MOVING;
                }
                else
                {
                    currentWaypoint--;
                    state = ELEVATORSTATE.MOVING;
                }
            }
        }
    }
}
