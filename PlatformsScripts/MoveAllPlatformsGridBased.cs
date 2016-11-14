using UnityEngine;
using System.Collections.Generic;

public class MoveAllPlatformsGridBased : MonoBehaviour
{

    public enum PLATFORMSTATE
    {
        IDLE,
        WAITING,
        MOVING
    }

    [Header("Elevator and Moving Platform")]
    [Tooltip("How many grids to move. If wanting to go down use a negative number. Make sure the platform is tagged")]
    public int gridsToMove = 2;
    [Tooltip("Amount of time the platform will stay in this spot before moving again")]
    public float waitTime = 2.0f;
    [Tooltip("If platform should be moving automatically which a switch/pad to enable tick this")]
    public bool automatic = false;

    [Header("Platform Move Speed")]
    [Tooltip("How fast the platforms should move")]
    public float moveSpeed = 2;

    [Header("Sounds")]
    public AudioClip elevatorSound;
    public AudioClip movingPlatformSound;
    public AudioClip bridgeSound;
    [Header("Sound Volumes")]
    [Range(0, 1)]
    public float evelatorBridgeSoundVolume = 1;
    [Range(0, 1)]
    public float movingPlatformSoundVolume = 1;


    [HideInInspector]
    public PLATFORMSTATE platformState;
    [HideInInspector]
    public GameObject[] objects;
    [HideInInspector]
    public Vector3 target;
    [HideInInspector]
    public bool activated = false;

    [HideInInspector]
    public Vector3 basePosition;
    [HideInInspector]
    public Vector3 secondPosition;
    [HideInInspector]
    public float lerpMoveTime = 0.0f;

    private AudioSource elevatorSource;
    private AudioSource platformSource;
    private AudioSource bridgeSource;

    private GridManager gridManager;
    
    private float gridSize = 1.25f;
    private float timer = 0.0f;



    // Use this for initialization
    void Start()
    {
        gridManager = GameObject.FindWithTag("Game Overseer").GetComponent<GridManager>();
        basePosition = this.transform.position;
        platformState = PLATFORMSTATE.IDLE;

        // set audio source for elevator
        if (this.transform.CompareTag("Elevator"))
            elevatorSource = this.transform.gameObject.GetComponent<AudioSource>();
        else
            elevatorSource = null;
        // set audio source for platforms
        if (this.transform.CompareTag("MovingPlatformX") || this.transform.CompareTag("MovingPlatformZ"))
            platformSource = this.transform.gameObject.GetComponent<AudioSource>();   
        else
            platformSource = null;
        // set audio source for bridge
        if (this.transform.CompareTag("Bridge"))
            bridgeSource = this.transform.gameObject.GetComponent<AudioSource>();

        // set second position to move to
        if (this.transform.CompareTag("Elevator"))
            secondPosition = new Vector3(basePosition.x, basePosition.y + (gridSize * gridsToMove), basePosition.z);
        else if(this.transform.CompareTag("MovingPlatformX"))
            secondPosition = new Vector3(basePosition.x + (gridSize * gridsToMove), basePosition.y , basePosition.z);
        else if (this.transform.CompareTag("MovingPlatformZ"))
            secondPosition = new Vector3(basePosition.x, basePosition.y, basePosition.z + (gridSize * gridsToMove));

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.CompareTag("Elevator") && !elevatorSource.isPlaying)
            elevatorSource.Play();
        else if ((this.transform.CompareTag("MovingPlatformX") || this.transform.CompareTag("MovingPlatformZ")) && !platformSource.isPlaying)
            platformSource.Play();
        else if (this.transform.CompareTag("Bridge"))
            bridgeSource.Play();

        if (platformState == PLATFORMSTATE.MOVING)
            UpdateMovingState();

        if (platformState == PLATFORMSTATE.IDLE)
            UpdateIdleState();

        if (platformState == PLATFORMSTATE.WAITING)
            UpdateWaitingState();


    }

    void UpdateWaitingState()
    {
        secondPosition = this.transform.position;

        foreach (Transform child in transform)
        {
            if (child.CompareTag("ShieldRobot"))
            {
                if (child.GetComponent<PlayerGridMovement>().playerState == PlayerGridMovement.PLAYERSTATE.IDLE)
                    timer += Time.deltaTime;
                else
                {
                    timer = 0.0f;
                    activated = false;
                    platformState = PLATFORMSTATE.WAITING;
                }
            }
            else
                timer += Time.deltaTime;
        }


        if (timer > waitTime)
        {
            timer = 0.0f;
            activated = false;
            platformState = PLATFORMSTATE.IDLE;
        }
    }

    void UpdateMovingState()
    {
        if (this.transform.CompareTag("Elevator"))
        {
            elevatorSource.volume = evelatorBridgeSoundVolume;
            elevatorSource.clip = elevatorSound;
        }
        else if (this.transform.CompareTag("MovingPlatformX") || this.transform.CompareTag("MovingPlatformZ"))
        {
            platformSource.volume = movingPlatformSoundVolume;
            platformSource.clip = movingPlatformSound;

            foreach (Transform child in transform)
            {
                if (child.CompareTag("Box"))
                {
                    child.GetComponent<BoxMove>().moveSpeed = moveSpeed;
                    child.GetComponent<BoxMove>().startLerpPos = child.transform.parent.position;
                    child.GetComponent<BoxMove>().target = new Vector3(target.x, child.transform.position.y, target.z);
                    child.GetComponent<BoxMove>().boxState = BoxMove.BOXSTATE.MOVING;

                }
            }
        }
        // move time
        lerpMoveTime += Time.deltaTime * moveSpeed;

        // if entered the trigger
        if (activated)
            target = secondPosition;
        // if exited the trigger
        else
            target = basePosition;

        
        

        // make it move
        transform.position = Vector3.Lerp(this.transform.position, target, lerpMoveTime);
        if (lerpMoveTime >= 1)
        {
            lerpMoveTime = 0;
            platformState = PLATFORMSTATE.IDLE;
        }
    }

    void UpdateIdleState()
    {
        if (this.transform.CompareTag("Elevator"))      
            elevatorSource.clip = null;
        else if (this.transform.CompareTag("MovingPlatformX") || this.transform.CompareTag("MovingPlatformZ"))
            platformSource.clip = null;
        else if (this.transform.CompareTag("Bridge"))
            bridgeSource.clip = null;

        return;
        
    }
}
