using UnityEngine;
using System.Collections;

public class BoxAheadTrigger : MonoBehaviour {


    public float pDelay = 0.5f;
    public Animator anim;

    private SwapCharacter currentCharacter;
    private GridManager gridManager;
    private bool boxFound = false;
    private bool canPush = true;
    private float gridSize = 1.25f;
    


    // Use this for initialization
    void Start () {
        gridManager = GameObject.FindWithTag("Game Overseer").GetComponent<GridManager>();
        currentCharacter = GameObject.FindWithTag("Game Overseer").GetComponent<SwapCharacter>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Fire2") &&
            GameObject.FindWithTag("Game Overseer").GetComponent<SwapCharacter>().currentPlayer.CompareTag("ShieldRobot") &&
            currentCharacter.currentPlayer.CompareTag("ShieldRobot") &&
            currentCharacter.currentPlayer.GetComponent<PlayerGridMovement>().playerState == PlayerGridMovement.PLAYERSTATE.IDLE &&
            canPush == true)
        {
            
            // make sure there is a box ahead of the player before doing anything else     
            var gridAhead = gridManager.PosToIndex(this.transform.position + (transform.forward * gridSize));            
            if (gridManager.TileHasObjectWithTag("Box", gridAhead))
            {
                var gridAheadTwo = gridManager.PosToIndex(this.transform.position + (transform.forward * (gridSize * 2)));
                Debug.Log(gridManager.PosToIndex(this.transform.position));
                Debug.Log("Ahead two: " + gridAheadTwo);

                if (!gridManager.TileHasObjectWithTag(new string[] { "Box", "Wall", "AgileRobot", "Turret" }, gridAheadTwo))
                {
                    var box = gridManager.GameObjectOnTileAtIndex("Box", gridAhead);
                    var boxDirToPlayer = box.transform.position - new Vector3(this.transform.position.x, box.transform.position.y, this.transform.position.z);

                    if (boxDirToPlayer.x >= 0.9)
                        boxDirToPlayer = new Vector3(1, 0, 0);
                    else if (boxDirToPlayer.x <= -0.9)
                        boxDirToPlayer = new Vector3(-1, 0, 0);
                    else if (boxDirToPlayer.z >= 0.9)
                        boxDirToPlayer = new Vector3(0, 0, 1);
                    else if (boxDirToPlayer.z <= -0.9)
                        boxDirToPlayer = new Vector3(0, 0, -1);

                    // set direction for box to move
                    Vector3 boxDirToMove = box.transform.position + (boxDirToPlayer * gridSize);
                    // tell box to try move
                    box.GetComponent<BoxMove>().MoveBox(boxDirToMove);
                    // do animations
                    if(box.GetComponent<BoxMove>().canBoxMove)
                    {
                        StartCoroutine(PushDelay(pDelay));
                        box.GetComponent<BoxMove>().canBoxMove = false;
                    }
                    
                    
                   
                }
            }
        }
	}
    public IEnumerator PushDelay(float d)
    {
        canPush = false;
        anim.SetBool("billyPush", true);
        yield return new WaitForSecondsRealtime(d);
        anim.SetBool("billyPush", false);
        canPush = true;
    }
}
