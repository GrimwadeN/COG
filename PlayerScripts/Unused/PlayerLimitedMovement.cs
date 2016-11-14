using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// Basic player movement based off the x and z axis of a joystick or keyboard.
/// Speed value should be adjusted as required to suit gameplay.
/// This is setup so that the player can only move forward or backwards and not side to side.
/// Buttons are also setup to turn the player 45 degrees to simulate grid like movement.
/// This may need to be changed later to joystick control.
/// 
/// </summary>
/// 
public class PlayerLimitedMovement : MonoBehaviour {

    // set default direction to 0
    Vector3 direction = Vector3.zero;
    // movement speed
    [SerializeField]
    [Tooltip("The speed value for the movement of this robot")]
    float speed = 5;

#pragma warning disable CS0414 // The field 'PlayerLimitedMovement.rotationValue' is assigned but its value is never used
    float rotationValue = 0;
#pragma warning restore CS0414 // The field 'PlayerLimitedMovement.rotationValue' is assigned but its value is never used

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

        PlayerMove();
        PlayerTurn();

    }
    void PlayerMove()
    {
        // forward only movement direction vector based off axis controls. Can be used for joystick/keyboard
        if(Input.GetAxis("Vertical") >= 0.01f)
        {
            this.transform.position += this.transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetAxis("Vertical") <= -0.01f)
        {
            this.transform.position -= this.transform.forward * speed * Time.deltaTime;
        }

    }

    void PlayerTurn()
    {
        // Turn 90 degrees
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Rotate( new Vector3(0, -45, 0));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, 45, 0));
        }
    }

}
