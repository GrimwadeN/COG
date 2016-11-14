using UnityEngine;
using System.Collections;

public class CameraPositionToPlayer : MonoBehaviour {

    public Vector3 positionToPlayer = new Vector3(10.8f, -15.1f, 12.7f);
    private GameObject gridManager;
    private Vector3 targetPosition;
    private Vector3 currentPosition;
    private Vector3 velocity = Vector3.zero;
    private float smooth = 0.3f;
    private SwapCharacter swap;

	// Use this for initialization
	void Start () {
        gridManager = GameObject.FindWithTag("Game Overseer");
        swap = gridManager.GetComponent<SwapCharacter>();
    }
	
	// Update is called once per frame
	void Update () {

        targetPosition = swap.currentPlayer.transform.position - positionToPlayer;
        FollowPlayer();
        
	}

    void FollowPlayer()
    {        
        currentPosition = this.transform.position;

        transform.position = Vector3.SmoothDamp(currentPosition, targetPosition, ref velocity, smooth);

    }
}
