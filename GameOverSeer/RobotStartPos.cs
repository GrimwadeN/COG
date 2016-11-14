using UnityEngine;
using System.Collections;

public class RobotStartPos : MonoBehaviour {

    [HideInInspector]
    public Vector3 mandyStartPos;
    [HideInInspector]
    public Vector3 billyStartPos;

	// Use this for initialization
	void Start () {
        mandyStartPos = GameObject.FindWithTag("AgileRobot").transform.position;
        billyStartPos = GameObject.FindWithTag("ShieldRobot").transform.position;
	}
	

}
