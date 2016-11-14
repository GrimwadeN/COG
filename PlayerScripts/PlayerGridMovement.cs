using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGridMovement : MonoBehaviour {

    [HideInInspector]
    public enum PLAYERSTATE
    {
        IDLE,
        MOVING,
        JUMPING,
    }
    
    [HideInInspector]
    public PLAYERSTATE playerState;
    [HideInInspector]
    public Vector3 target;
    [HideInInspector]
    public Vector3 startLerpPos;
    [HideInInspector]
    public Vector3 center;
    [HideInInspector]
    public Vector3 startSlerpPos;
    [HideInInspector]
    public Vector3 endSlerpPos;
    [HideInInspector]
    public Vector3 faceLeft = new Vector3(0, -90, 0);
    [HideInInspector]
    public Vector3 faceRight = new Vector3(0, 90, 0);
    [HideInInspector]
    public Vector3 faceFront = new Vector3(0, 0, 0);
    [HideInInspector]
    public Vector3 faceBack = new Vector3(0, 180, 0);

    [Header("Movement")]
    public float moveSpeed = 2;

    [Header("Animation stuff")]
    public Animator anim;
    public Animator mandyAnim;
    public string animationName;
    public float mandyIdleSwapDelay = 1;
    

    [Header("Character Sound Tracks")]
    // billy's sounds
    public AudioClip idleBilly;
    public AudioClip movingBilly;
    // mandy's sounds
    public AudioClip idleMandy;
    public AudioClip movingMandy;
    public AudioClip jumpingMandy;


    [Header("Sound Volumes")]
    [Range(0, 1)]  
    public float billyIdleVolume = 1;
    [Range(0, 1)]
    public float billyMovingVolume = 1;
    [Range(0, 1)]
    public float mandyIdleVolume = 1;
    [Range(0, 1)]
    public float mandyMovingVolume = 1;
    [Range(0, 1)]
    public float mandyJumpingVolume = 1;

    [HideInInspector]
    public bool mandyInteracting = false;

    [HideInInspector]
    public bool hitByBullet = false;

    // current robot being used
    private SwapCharacter currentCharacter;
    // Audio sources
    private AudioSource billySound;
    private AudioSource mandySound;

    private GridManager gridManager;
    private Vector3 direction = Vector3.zero;     

#pragma warning disable CS0169 // The field 'PlayerGridMovement.gridAhead' is never used
    private Vector3 gridAhead;
#pragma warning restore CS0169 // The field 'PlayerGridMovement.gridAhead' is never used
#pragma warning disable CS0169 // The field 'PlayerGridMovement.gridRight' is never used
    private Vector3 gridRight;
#pragma warning restore CS0169 // The field 'PlayerGridMovement.gridRight' is never used
#pragma warning disable CS0169 // The field 'PlayerGridMovement.gridLeft' is never used
    private Vector3 gridLeft;
#pragma warning restore CS0169 // The field 'PlayerGridMovement.gridLeft' is never used
#pragma warning disable CS0169 // The field 'PlayerGridMovement.gridBack' is never used
    private Vector3 gridBack;
#pragma warning restore CS0169 // The field 'PlayerGridMovement.gridBack' is never used
    private Vector3 desiredGrid;
    private Vector3 currentIndex;
    private float gridSize = 1.25f;
    private float lerpMoveTime = 0.0f;
    private bool canIMoveOffElevator = true;
    private bool didIFindABox = false;
    private bool isObjectOnMyLevel = false;

    private Vector3 mandyStartPos;
    private GameObject mandy;

    // Use this for initialization
    void Start()
    {
        playerState = PLAYERSTATE.IDLE;
        gridManager = GameObject.FindWithTag("Game Overseer").GetComponent<GridManager>();
        currentIndex = gridManager.PosToIndex(this.transform.position);
        currentCharacter = gridManager.GetComponent<SwapCharacter>();

        billySound = GameObject.FindWithTag("ShieldRobot").GetComponent<AudioSource>();
        mandySound = GameObject.FindWithTag("AgileRobot").GetComponent<AudioSource>();

        mandy = GameObject.FindWithTag("AgileRobot");
    }

    // Update is called once per frame
    void Update()
    {


        // Play sounds if moving
        if (currentCharacter.currentPlayer.CompareTag("ShieldRobot") && !billySound.isPlaying)
        {
            billySound.Play();
            mandySound.Stop();
        }
            
        else if (currentCharacter.currentPlayer.CompareTag("AgileRobot") && !mandySound.isPlaying)
        {
            mandySound.Play();
            billySound.Stop();
        } 
        

        if (playerState == PLAYERSTATE.MOVING)
            UpdateMoveState();
        // if udate move state changed to idle this frame, also do the idle state update
        else if (playerState == PLAYERSTATE.IDLE)
            UpdateIdleState();
        else if (playerState == PLAYERSTATE.JUMPING)
            UpdateJumpState();

        if (this.transform.tag == "ShieldRobot" && this.transform.parent != null && this.transform.parent.CompareTag("Elevator"))
        {
            transform.position = new Vector3(transform.position.x, this.transform.parent.position.y - 0.533f, transform.position.z);
        }

        
    }
    

    void UpdateJumpState()
    {
        // jump sound
        if (currentCharacter.currentPlayer.CompareTag("AgileRobot") && gridManager.GetComponent<PauseUnPause>().paused == false)
        {
            mandySound.volume = mandyJumpingVolume;
            mandySound.clip = jumpingMandy;
        }
            

        lerpMoveTime += Time.deltaTime * moveSpeed;

        center = (startLerpPos + target) * 0.5f;
        center -= new Vector3(0, 1, 0);
        startSlerpPos = startLerpPos - center;
        endSlerpPos = target - center;

        transform.position = Vector3.Slerp(startSlerpPos, endSlerpPos, lerpMoveTime);
        transform.position += center;

        mandyAnim.SetBool("mandyJump", true);
        if (lerpMoveTime >= 1.0f)
        {
            currentIndex = gridManager.PosToIndex(transform.position);
            lerpMoveTime = 0.0f;
            mandyAnim.SetBool("mandyJump", false);
            playerState = PLAYERSTATE.IDLE;
        }

   
    }

    void UpdateMoveState()
    {
        this.transform.parent = null;
        // Play sounds if moving
        if (currentCharacter.currentPlayer.CompareTag("ShieldRobot") && gridManager.GetComponent<PauseUnPause>().paused == false)
        {
            billySound.volume = billyMovingVolume;
            billySound.clip = movingBilly;
        }            
        else if (currentCharacter.currentPlayer.CompareTag("AgileRobot") && gridManager.GetComponent<PauseUnPause>().paused == false)
        {
            mandySound.volume = mandyMovingVolume;
            mandySound.clip = movingMandy;
        }
        else
            Debug.Log("No character selected");

        lerpMoveTime += Time.deltaTime * moveSpeed;
        if (this.transform.CompareTag("ShieldRobot"))
            anim.SetBool(animationName, true);
        else
            mandyAnim.SetBool(animationName, true);

        // move to target position
        transform.position = Vector3.Lerp(startLerpPos, target, lerpMoveTime);
        // swap back to idle state to allow next move
        if (lerpMoveTime >= 1.0f)
        {
            currentIndex = gridManager.PosToIndex(transform.position);
            lerpMoveTime = 0.0f;
            InputCheck();          
        }
    }

    void UpdateIdleState()
    {
        if (this.transform.CompareTag("ShieldRobot") && anim.GetBool(animationName) == true)
            anim.SetBool(animationName, false);
        else if (this.transform.CompareTag("AgileRobot") && mandyAnim.GetBool(animationName) == true)
            mandyAnim.SetBool(animationName, false);

        StartCoroutine(PlayRandomAnimation(mandyIdleSwapDelay));

        // Play sounds if moving
        if (currentCharacter.currentPlayer.CompareTag("ShieldRobot") && gridManager.GetComponent<PauseUnPause>().paused == false)
        {
            billySound.volume = billyIdleVolume;
            billySound.clip = idleBilly;
        }            
        else if (currentCharacter.currentPlayer.CompareTag("AgileRobot") && gridManager.GetComponent<PauseUnPause>().paused == false)
        {
            mandySound.volume = mandyIdleVolume;
            mandySound.clip = idleMandy;
        }            
        else
            Debug.Log("No character selected");

        currentIndex = gridManager.PosToIndex(transform.position);

        if (this.transform.CompareTag("AgileRobot") && mandyInteracting == false)
            InputCheck();
        else if(this.transform.CompareTag("ShieldRobot"))
            InputCheck();
        
        if(this.transform.CompareTag("ShieldRobot"))
            anim.SetBool(animationName, false);

    }

    int RandomIdle()
    {
        return Random.Range(1, 500);
    }

    void InputCheck()
    {

        // center point of each grid rather than the grid position
        Vector3 gridPosOffset = new Vector3(gridSize, 0, gridSize) / 2.0f;
        // move left
        if ((Input.GetAxis("Horizontal") < -0.1f && Input.GetAxis("Vertical") < -0.6f) || Input.GetAxis("Horizontal") < -0.8f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));

            // check if it can move
            if (CanMove())
            {

                target = gridManager.IndexToPos(new Vector3(currentIndex.x - 1, currentIndex.y, currentIndex.z)) + gridPosOffset;
                target.y = this.transform.position.y;

                startLerpPos = gridManager.IndexToPos(currentIndex) + gridPosOffset;
                startLerpPos.y = transform.position.y;

                playerState = PLAYERSTATE.MOVING;
            }
            else
                playerState = PLAYERSTATE.IDLE;

        }
        // move forward
        else if ((Input.GetAxis("Vertical") > 0.1f && Input.GetAxis("Horizontal") < -0.6f) || Input.GetAxis("Vertical") > 0.8f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            // check if it can move
            if (CanMove())
            {

                target = gridManager.IndexToPos(new Vector3(currentIndex.x, currentIndex.y, currentIndex.z + 1)) + gridPosOffset;
                target.y = this.transform.position.y;

                startLerpPos = gridManager.IndexToPos(currentIndex) + gridPosOffset;
                startLerpPos.y = transform.position.y;
                playerState = PLAYERSTATE.MOVING;
            }
            else
                playerState = PLAYERSTATE.IDLE;
        }
        // move back
        else if ((Input.GetAxis("Vertical") < -0.1f && Input.GetAxis("Horizontal") > 0.6f) || Input.GetAxis("Vertical") < -0.8f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            //check if it can move
            if (CanMove())
            {

                target = gridManager.IndexToPos(new Vector3(currentIndex.x, currentIndex.y, currentIndex.z - 1)) + gridPosOffset;
                target.y = this.transform.position.y;

                startLerpPos = gridManager.IndexToPos(currentIndex) + gridPosOffset;
                startLerpPos.y = transform.position.y;

                playerState = PLAYERSTATE.MOVING;

            }
            else
                playerState = PLAYERSTATE.IDLE;
        }
        // move right
        else if ((Input.GetAxis("Horizontal") > 0.1f && Input.GetAxis("Vertical") > 0.6f) || Input.GetAxis("Horizontal") > 0.8f)
        {

            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            // check if it can move
            if (CanMove())
            {
                
                target = gridManager.IndexToPos(new Vector3(currentIndex.x + 1, currentIndex.y, currentIndex.z)) + gridPosOffset;
                target.y = this.transform.position.y;

                startLerpPos = gridManager.IndexToPos(currentIndex) + gridPosOffset;
                startLerpPos.y = transform.position.y;

                playerState = PLAYERSTATE.MOVING;
            }
            else
                playerState = PLAYERSTATE.IDLE;
        }
        else
        {
            playerState = PLAYERSTATE.IDLE;
        }
            
    }
    // See if there is anything blocking the player from moving
    bool CanMove()
    {

        desiredGrid = gridManager.PosToIndex(this.transform.position + (transform.forward * gridSize));

        if (this.transform.parent != null && this.transform.parent.CompareTag("Bridge"))
            if(this.transform.parent.parent.CompareTag("Elevator"))
                if (this.transform.parent.parent.GetComponent<MoveAllPlatformsGridBased>().platformState == MoveAllPlatformsGridBased.PLATFORMSTATE.MOVING)
                    return false;
        else if (this.transform.parent != null && 
        (this.transform.parent.CompareTag("Elevator") ||
        this.transform.parent.CompareTag("MovingPlatformX") ||
        this.transform.parent.CompareTag("MovingPlatformZ")) &&
        this.transform.parent.GetComponent<MoveAllPlatformsGridBased>().platformState == MoveAllPlatformsGridBased.PLATFORMSTATE.MOVING)
             return false;



        var test = gridManager.PosToIndex(this.transform.position);

        desiredGrid = gridManager.PosToIndex(this.transform.position + (transform.forward * gridSize));

        if (CheckObjectsInDirection(desiredGrid)) return true;
        else return false;
 
    }  
    

    bool CheckObjectsInDirection(Vector3 grid)
    {
        isObjectOnMyLevel = false;
        didIFindABox = false;
        canIMoveOffElevator = true;

        var boxAheadBelowGrid = new Vector3(grid.x, grid.y - 1, grid.z);

        // check if there is a box + floor tile in the desired grid
        if(gridManager.GameObjectOnTileAtIndex(new string[] { "Box", "Turret", "Wall", "AgileRobot", "ShieldRobot" }, grid))
        {
            didIFindABox = true;
        }    
        else
            didIFindABox = false;

        var bridge = gridManager.GameObjectOnTileAtIndex("Bridge", grid);
        if(bridge != null)
        {
            if (bridge.transform.parent.GetComponent<MoveAllPlatformsGridBased>().platformState == MoveAllPlatformsGridBased.PLATFORMSTATE.MOVING)
            {
                canIMoveOffElevator = false;
            }
        }
        


        string[] moveOnTopOfObjects = { "Floor", "Elevator", "MovingPlatformX", "MovingPlatformZ", "Bridge", "Switch" };
        // check if theres a box to replace a floor tile in grid ahead but down 1.
        if (gridManager.GameObjectOnTileAtIndex(moveOnTopOfObjects, grid))
        {
            isObjectOnMyLevel = true;
            if(gridManager.GameObjectOnTileAtIndex(moveOnTopOfObjects, grid).transform.FindChild("MoveableBox"))
            {
                didIFindABox = true;
            }
        }
            
        else if (gridManager.TileHasObjectWithTag("Box", boxAheadBelowGrid))
        {
            if(gridManager.GameObjectOnTileAtIndex("Box", boxAheadBelowGrid).transform.GetComponent<BoxMove>().boxState == BoxMove.BOXSTATE.IDLE)
                isObjectOnMyLevel = true;
        }
            
        else
            isObjectOnMyLevel = false;
     
        if(transform.parent != null)
        {
            // if its a box
            if(transform.parent.CompareTag("Box"))
            {
                canIMoveOffElevator = true;
            }
            // if its a bridge
            else if(this.transform.parent.CompareTag("Bridge"))
            {
                if (this.transform.parent.parent.GetComponent<MoveAllPlatformsGridBased>().platformState == MoveAllPlatformsGridBased.PLATFORMSTATE.IDLE)
                    canIMoveOffElevator = true;
                else
                    canIMoveOffElevator = false;
            }
            // if its moving platform or elevator
            else if(this.transform.parent.CompareTag("MovingPlatformX") || this.transform.parent.CompareTag("MovingPlatformZ") || this.transform.parent.CompareTag("Elevator"))
            {
                if ((transform.parent.GetComponent<MoveAllPlatformsGridBased>().platformState == MoveAllPlatformsGridBased.PLATFORMSTATE.WAITING ||
                transform.parent.GetComponent<MoveAllPlatformsGridBased>().platformState == MoveAllPlatformsGridBased.PLATFORMSTATE.IDLE) &&
                (gridManager.GameObjectOnTileAtIndex(moveOnTopOfObjects, grid) ||
                gridManager.GameObjectOnTileAtIndex("Box", boxAheadBelowGrid)))
                {
                    canIMoveOffElevator = true;
                }
            }
            else
                canIMoveOffElevator = false;
        }

        // use checks to determine move
        if (didIFindABox == false && isObjectOnMyLevel == true && canIMoveOffElevator == true)
            return true;
        else
            return false;

    }

    public void MandyReset()
    {
        mandyStartPos = GameObject.Find("GameOverseer").GetComponent<RobotStartPos>().mandyStartPos;
        this.transform.parent = null;
        string[] tags = { "Box", "AgileRobot", "ShieldRobot" };
        // if the box's starting position has something there, swap its position slightly
        if (gridManager.GameObjectOnTileAtIndex(tags, gridManager.PosToIndex(mandyStartPos)))
        {
            var gridForward = new Vector3(mandyStartPos.x, mandyStartPos.y, mandyStartPos.z + gridSize);
            var gridBack = new Vector3(mandyStartPos.x, mandyStartPos.y, mandyStartPos.z - gridSize);
            var gridLeft = new Vector3(mandyStartPos.x - gridSize, mandyStartPos.y, mandyStartPos.z);
            var gridRight = new Vector3(mandyStartPos.x + gridSize, mandyStartPos.y, mandyStartPos.z);

            var gridPositions = new List<Vector3>();
            gridPositions.Add(gridForward);
            gridPositions.Add(gridBack);
            gridPositions.Add(gridLeft);
            gridPositions.Add(gridRight);

            // choose which position it will be at if something is at its start pos
            foreach (Vector3 pos in gridPositions)
            {
                if (gridManager.GameObjectOnTileAtIndex(tags, pos) == false)
                {
                    this.transform.position = pos;
                    break;
                }
            }
        }
        else
        {
            // set position to be the inital start position
            Debug.Log("Current Pos: " + this.transform.position);
            Debug.Log("Reset Pos: " + mandyStartPos);
            mandy.GetComponent<PlayerGridMovement>().playerState = PLAYERSTATE.IDLE;
            mandy.transform.position = mandyStartPos;
            //StartCoroutine(ResetPlayer());
        }
    }
  
    public IEnumerator ResetPlayer()
    {
        yield return new WaitForEndOfFrame();
        
       // Debug.Log("got past position reset");
    }

    public IEnumerator PlayRandomAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        if(this.transform.CompareTag("AgileRobot"))
            mandyAnim.SetInteger("mandyIdleRandom", RandomIdle());
        else if(this.transform.CompareTag("ShieldRobot"))
            anim.SetInteger("billyIdleRandom", RandomIdle());

    }
}


