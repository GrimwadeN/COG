using UnityEngine;
using System.Collections;

public class MandyThrusters : MonoBehaviour {

    private PlayerGridMovement mandyMove;
    private GameObject[] thrusters;

	// Use this for initialization
	void Start () {
        mandyMove = GameObject.FindWithTag("AgileRobot").GetComponent<PlayerGridMovement>();

        thrusters = GameObject.FindGameObjectsWithTag("Thruster");
	}
	
	// Update is called once per frame
	void Update () {
	    
        if(mandyMove.playerState == PlayerGridMovement.PLAYERSTATE.MOVING)
        {
            foreach (GameObject t in thrusters)
                t.SetActive(true);
        }
        else if(mandyMove.playerState == PlayerGridMovement.PLAYERSTATE.IDLE)
        {
            foreach (GameObject t in thrusters)
                t.SetActive(false);
        }
	}
}
