using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BoxMove : MonoBehaviour {
    public enum MOVEDIRECTION
    {
        BACK,
        FORWARD,
        LEFT,
        RIGHT
    }

    public enum BOXSTATE
    {
        IDLE,
        MOVING,
        FALLING
    }

    public MOVEDIRECTION movedirection;
    
    [Header("Box Move Info")]
    public float moveSpeed = 4;
    public bool canMove = true;

    [Header("Sound")]
    public AudioClip boxSound;
    [Range(0, 1)]
    public float boxVolume = 1;
    [HideInInspector]
    public BOXSTATE boxState;
    [HideInInspector]
    public Vector3 childTargetPos;
    private AudioSource boxSource;
    [HideInInspector]
    public Vector3 target = Vector3.zero;
    [HideInInspector]
    public Vector3 startLerpPos = Vector3.zero;
    private GridManager gridManager;
    [HideInInspector]
    public bool canBoxMove = false;
    
    private float gridSize = 1.25f;
    private float lerpMoveTime = 0.0f;
    private float fallingCount = 0;
    
    private Vector3 desiredGrid = Vector3.zero;
    private Vector3 startPosition;
    private bool hasBeenPushed = false;
    
    // Use this for initialization
    void Start () {
        boxState = BOXSTATE.IDLE;
        gridManager = GameObject.FindWithTag("Game Overseer").GetComponent<GridManager>();
        startPosition = this.transform.position;

        boxSource = this.transform.GetComponent<AudioSource>();


    }
	
	// Update is called once per frame
	void Update () {

        if(boxState == BOXSTATE.MOVING)
        {
            if(hasBeenPushed == false)
            {
                boxSource.PlayOneShot(boxSound, boxVolume);
                hasBeenPushed = true;
            }
            // set childs target position
            foreach (Transform child in this.transform)
            {
                if (child.CompareTag("Box"))
                {
                    Vector3 currentPos = this.transform.position;
                    child.GetComponent<BoxMove>().startLerpPos = new Vector3(currentPos.x, currentPos.y + gridSize, currentPos.z);
                    child.GetComponent<BoxMove>().childTargetPos = new Vector3(target.x, target.y + gridSize, target.z);
                }
            }
            // set the target position if the box has a parent box
            if (this.transform.parent != null && this.transform.parent.CompareTag("Box"))
                target = childTargetPos;
            else if (this.transform.parent != null && this.transform.parent.CompareTag("Elevator") )
            {
                Vector3 parentPos = this.transform.parent.position;
                if (this.transform.parent.GetComponent<MoveAllPlatformsGridBased>().platformState == MoveAllPlatformsGridBased.PLATFORMSTATE.MOVING)
                    target = new Vector3(this.transform.parent.GetComponent<MoveAllPlatformsGridBased>().target.x, this.transform.parent.GetComponent<MoveAllPlatformsGridBased>().target.y, this.transform.parent.GetComponent<MoveAllPlatformsGridBased>().target.z);

            }


            // move
            lerpMoveTime += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startLerpPos, target, lerpMoveTime);
            if (lerpMoveTime >= 1.0f)
            {             
                lerpMoveTime = 0.0f;
                hasBeenPushed = false;
                boxState = BOXSTATE.IDLE;
            }          
        }
        else if(boxState == BOXSTATE.FALLING)
        {

            fallingCount += Time.deltaTime;
            lerpMoveTime += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startLerpPos, target, lerpMoveTime);

            // if falling forever reset
            if (fallingCount >= 2)
            {
                if (this.transform.parent != null)
                    IsFalling();
                if (this.transform.FindChild("Mandy"))
                    GameObject.FindWithTag("AgileRobot").GetComponent<PlayerGridMovement>().MandyReset();
                else
                    BoxReset();
            }

            // Fall
            if (lerpMoveTime >= 1.0f)
            {
                lerpMoveTime = 0.0f;
                IsFalling();                  
            }
        }
        else if(boxState == BOXSTATE.IDLE)
        {
            // if box is on an elevator make sure it moves with  it
            if (this.transform.parent != null && (this.transform.parent.CompareTag("Elevator") ))
            {
                transform.position = new Vector3(transform.position.x, this.transform.parent.position.y, transform.position.z);
                if (transform.parent.GetComponent<MoveAllPlatformsGridBased>().platformState == MoveAllPlatformsGridBased.PLATFORMSTATE.MOVING)
                {
                    MoveBox(transform.parent.GetComponent<MoveAllPlatformsGridBased>().target);
                }
            }
            else if(this.transform.parent != null && this.transform.parent.CompareTag("Box"))
            {
                if (this.transform.parent.GetComponent<BoxMove>().boxState == BOXSTATE.MOVING)
                    boxState = BOXSTATE.MOVING;
                else if (this.transform.parent.GetComponent<BoxMove>().boxState == BOXSTATE.FALLING)
                    boxState = BOXSTATE.FALLING;
            }

            else
            {
                // Once moved forward make sure floor is under the box
                if (this.transform.parent != null && this.transform.parent.CompareTag("Box"))
                    StartCoroutine(FallDelay());
                else
                    IsFalling();
            }
                
        }
	}


    
   

    void BoxReset()
    {

        string[] tags = { "Box", "AgileRobot", "ShieldRobot" };
        // if the box's starting position has something there, swap its position slightly
        if (gridManager.GameObjectOnTileAtIndex(tags, gridManager.PosToIndex(startPosition)))
        {
            var gridForward = new Vector3(startPosition.x, startPosition.y, startPosition.z + gridSize);
            var gridBack = new Vector3(startPosition.x, startPosition.y, startPosition.z - gridSize);
            var gridLeft = new Vector3(startPosition.x - gridSize, startPosition.y, startPosition.z);
            var gridRight = new Vector3(startPosition.x + gridSize, startPosition.y, startPosition.z);

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
                    foreach(Transform child in this.transform)
                    {
                        if(child.CompareTag("Box"))
                        {
                            child.transform.position = new Vector3(pos.x, pos.y + gridSize, pos.z);
                            child.GetComponent<BoxMove>().fallingCount = 0;
                            child.GetComponent<BoxMove>().boxState = BOXSTATE.IDLE;
                            fallingCount = 0;
                        }
                    } 
                }
            }
        }
        else
        {
            this.transform.position = startPosition;
        }

        fallingCount = 0;
        boxState = BOXSTATE.IDLE;
    }

    void IsFalling()
    {
        // check if its falling or not
        var boxGrid = gridManager.PosToIndex(this.transform.position);
        var gridBelow = gridManager.PosToIndex(new Vector3(this.transform.position.x, this.transform.position.y - gridSize, this.transform.position.z));
        if (gridManager.TileHasObjectWithTag(new string[] { "Floor", "Elevator", "Switch", "Bridge", "MovingPlatformX", "MovingPlatformZ" }, boxGrid) == false &&
            gridManager.TileHasObjectWithTag("Box", gridBelow) == false)       
        {

            startLerpPos = transform.position;
            target = startLerpPos + (-transform.up * gridSize);
            
            boxState = BOXSTATE.FALLING;
        }
        else
            boxState = BOXSTATE.IDLE;
    }

    public void MoveBox(Vector3 index)
    {
        var gridAhead = gridManager.PosToIndex(index);
        // check if it has a parent so it doesn't stay still if the parent moves and set the target
        if( this.transform.parent != null && 
            (this.transform.parent.CompareTag("Elevator") || this.transform.parent.CompareTag("MovingPlatformX") || this.transform.parent.CompareTag("MovingPlatformZ")))
        {         
            target = index;
            startLerpPos = this.transform.position;
            canBoxMove = true;
            boxState = BOXSTATE.MOVING;
        }
        // set target position to move to
        else if(gridManager.TileHasObjectWithTag(new string[] { "AgileRobot", "Box", "Wall", "Turret"}, gridAhead) == false)
        {
            target = index;
            startLerpPos = this.transform.position;
            canBoxMove = true;
            boxState = BOXSTATE.MOVING;
        }  
    }

    public IEnumerator FallDelay()
    {
        yield return new WaitForEndOfFrame();
        IsFalling();
    }
}

