using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// Basic player movement based off the x and z axis of a joystick or keyboard.
/// Speed value should be adjusted as required to suit gameplay.
/// This is setup so that the player can only move forward or backwards and not side to side.
/// 
/// </summary>

public class PlayerMovement : MonoBehaviour {

    // set default direction to 0
    Vector3 direction = Vector3.zero;
    [Header("Player Controls")]
    // movement speed
    [SerializeField]
    [Tooltip("The speed value for the movement of this robot")]
    float speed = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        PlayerMove();

	}

    void PlayerMove()
    {
        // direction vector based off axis controls. Can be used for joystick/keyboard
        direction = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal"));
        // apply the movement
        this.transform.position += direction * speed * Time.deltaTime;

    }
}
