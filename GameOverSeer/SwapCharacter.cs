using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// This script is setup to swap control between the robots to allow for the problem solving.
/// 
/// </summary>

public class SwapCharacter : MonoBehaviour {
    [HideInInspector]
    public GameObject currentPlayer;

    private GameObject agileRobot;
    private GameObject tankRobot;

    
	// Use this for initialization
	void Start () {
        agileRobot = GameObject.FindWithTag("AgileRobot");
        tankRobot = GameObject.FindWithTag("ShieldRobot");
        currentPlayer = tankRobot;
        agileRobot.GetComponent<PlayerGridMovement>().enabled = false;
        agileRobot.GetComponent<AgileJumpScript>().enabled = false;


        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.2f * Time.timeScale;
    }
	
	// Update is called once per frame
	void Update () {

        if(currentPlayer == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
	    
        if(currentPlayer == tankRobot)
        {
            if(Input.GetButtonDown("Fire1") && currentPlayer.GetComponent<PlayerGridMovement>().playerState == PlayerGridMovement.PLAYERSTATE.IDLE)
            {
                tankRobot.GetComponent<PlayerGridMovement>().enabled = false;
                agileRobot.GetComponent<PlayerGridMovement>().enabled = true;
                agileRobot.GetComponent<AgileJumpScript>().enabled = true;
                currentPlayer = agileRobot;
            }
        }
        else if(currentPlayer == agileRobot)
        {
            if (Input.GetButtonDown("Fire1") && currentPlayer.GetComponent<PlayerGridMovement>().playerState == PlayerGridMovement.PLAYERSTATE.IDLE && currentPlayer.GetComponent<PlayerGridMovement>().mandyInteracting == false )
            {
                agileRobot.GetComponent<AgileJumpScript>().enabled = false;
                agileRobot.GetComponent<PlayerGridMovement>().enabled = false;
                tankRobot.GetComponent<PlayerGridMovement>().enabled = true;
                
                currentPlayer = tankRobot;
            }
        }

	}
}
