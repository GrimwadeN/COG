using UnityEngine;
using System.Collections;

public class AgileJumpScript : MonoBehaviour
{
    private GameObject gameOverseer;
    private PlayerGridMovement player;

    private float gridSize = 1.25f;
    
    // Use this for initialization
    void Start()
    {
        gameOverseer = GameObject.FindWithTag("Game Overseer");
        player = GameObject.FindWithTag("AgileRobot").GetComponent<PlayerGridMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetButtonDown("Fire3") && this.transform.GetComponent<PlayerGridMovement>().playerState == PlayerGridMovement.PLAYERSTATE.IDLE))
        {
            GetJumpType();      
        }
    }

    void GetJumpType()
    {
        // Tile locations to check for objects
        var gridManager = gameOverseer.GetComponent<GridManager>();
        var playerGridIndex = gridManager.PosToIndex(transform.position);
        var gridAhead = gridManager.PosToIndex(this.transform.position + (transform.forward * gridSize));
        var gridAheadTwo = gridManager.PosToIndex(JumpGapTarget());
        var gridUp = gridManager.PosToIndex(JumpUpTarget());
        var gridDown = gridManager.PosToIndex(JumpDownTarget());
        var gridDownTwo = gridManager.PosToIndex(JumpDownTwoTarget());

        string[] testFloorTiles = { "Floor", "MovingPlatformX", "MovingPlatformZ", "Switch", "Elevator", "Bridge" };

        // if there is a box ahead  jump up
        if((gridManager.TileHasObjectWithTag(testFloorTiles, gridUp) ||
           gridManager.TileHasObjectWithTag("Box", gridAhead)) &&
           gridManager.TileHasObjectWithTag(new string[] { "Box", "ShieldRobot", "Turret", "Wall" }, gridUp) == false)      
        {
            // if box ahead make sure it isn't falling
            if (gridManager.TileHasObjectWithTag("Box", gridAhead))
            {
                if (gridManager.GameObjectOnTileAtIndex("Box", gridAhead).transform.GetComponent<BoxMove>().boxState == BoxMove.BOXSTATE.IDLE)
                {
                    player.target = JumpUpTarget();
                    player.startLerpPos = this.transform.position;
                    player.playerState = PlayerGridMovement.PLAYERSTATE.JUMPING;
                }
            }
            else
            {
                player.target = JumpUpTarget();
                player.startLerpPos = this.transform.position;
                player.playerState = PlayerGridMovement.PLAYERSTATE.JUMPING;
            }
        }
        // Jump the gap!     
        else if (gridManager.TileHasObjectWithTag(new string[] { "Box", "ShieldRobot" }, gridAhead) == false &&
           gridManager.TileHasObjectWithTag(new string[] { "Box", "ShieldRobot" }, gridAheadTwo) == false &&
           gridManager.TileHasObjectWithTag(new string[] { "Elevator", "Floor", "MovingPlatformX", "MovingPlatformZ"}, gridUp) == false &&
           gridManager.TileHasObjectWithTag(new string[] { "Floor" , "Elevator", "MovingPlatformX", "MovingPlatformZ", "Switch", "Bridge" }, gridAheadTwo))
        {
            player.target = JumpGapTarget();
            player.startLerpPos = this.transform.position;
            player.playerState = PlayerGridMovement.PLAYERSTATE.JUMPING;
        }
        // jump down
        else if((gridManager.TileHasObjectWithTag(testFloorTiles, gridDown) ||
                 gridManager.TileHasObjectWithTag("Box", gridDownTwo)) &&
                gridManager.TileHasObjectWithTag(testFloorTiles, gridAhead) == false &&
                gridManager.TileHasObjectWithTag(new string[] { "Box", "ShieldRobot", "Turret", "Wall" }, gridDown) == false)
        {
            // check if theres a box to jump down to.
            if(gridManager.TileHasObjectWithTag("Box", gridDownTwo))
            {
                // make sure it isn't falling
                if(gridManager.GameObjectOnTileAtIndex("Box", gridDownTwo).transform.GetComponent<BoxMove>().boxState == BoxMove.BOXSTATE.IDLE)
                {
                    player.target = JumpDownTarget();
                    player.startLerpPos = this.transform.position;
                    player.playerState = PlayerGridMovement.PLAYERSTATE.JUMPING;
                }
            }
            // if no box, but other stuff, do this
            else
            {
                player.target = JumpDownTarget();
                player.startLerpPos = this.transform.position;
                player.playerState = PlayerGridMovement.PLAYERSTATE.JUMPING;
            }
            
        }
    }

    Vector3 JumpDownTwoTarget()
    {
        if (transform.rotation == Quaternion.Euler(player.faceBack))
            return new Vector3(this.transform.position.x, this.transform.position.y - (gridSize * 2), this.transform.position.z - gridSize);
        else if (transform.rotation == Quaternion.Euler(player.faceFront))
            return new Vector3(this.transform.position.x, this.transform.position.y - (gridSize * 2), this.transform.position.z + gridSize);
        else if (transform.rotation == Quaternion.Euler(player.faceRight))
            return new Vector3(this.transform.position.x + gridSize, this.transform.position.y - (gridSize * 2), this.transform.position.z );
        else if (transform.rotation == Quaternion.Euler(player.faceLeft))
            return new Vector3(this.transform.position.x - gridSize , this.transform.position.y - (gridSize * 2), this.transform.position.z);

        return new Vector3(0, 0, 0);
    }

    Vector3 JumpUpTarget()
    {
        if (transform.rotation == Quaternion.Euler(player.faceBack))
            return new Vector3(this.transform.position.x, this.transform.position.y + gridSize, this.transform.position.z - gridSize);
        else if (transform.rotation == Quaternion.Euler(player.faceFront))
            return new Vector3(this.transform.position.x, this.transform.position.y + gridSize, this.transform.position.z + gridSize);
        else if (transform.rotation == Quaternion.Euler(player.faceRight))
            return new Vector3(this.transform.position.x + gridSize, this.transform.position.y + gridSize, this.transform.position.z);
        else if (transform.rotation == Quaternion.Euler(player.faceLeft))
            return new Vector3(this.transform.position.x - gridSize, this.transform.position.y + gridSize, this.transform.position.z);

        return new Vector3(0, 0, 0);
    }

    Vector3 JumpDownTarget()
    {
        if (transform.rotation == Quaternion.Euler(player.faceBack))
            return new Vector3(this.transform.position.x, this.transform.position.y - gridSize, this.transform.position.z - gridSize);
        else if (transform.rotation == Quaternion.Euler(player.faceFront))
            return new Vector3(this.transform.position.x, this.transform.position.y - gridSize, this.transform.position.z + gridSize);
        else if (transform.rotation == Quaternion.Euler(player.faceRight))
            return new Vector3(this.transform.position.x + gridSize, this.transform.position.y - gridSize, this.transform.position.z);
        else if (transform.rotation == Quaternion.Euler(player.faceLeft))
            return new Vector3(this.transform.position.x - gridSize, this.transform.position.y - gridSize, this.transform.position.z);

        return new Vector3(0, 0, 0);
    }

    Vector3 JumpGapTarget()
    {
        if (transform.rotation == Quaternion.Euler(player.faceBack))
            return new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - (gridSize * 2));
        else if (transform.rotation == Quaternion.Euler(player.faceFront))
            return new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + (gridSize * 2));
        else if (transform.rotation == Quaternion.Euler(player.faceRight))
            return new Vector3(this.transform.position.x + (gridSize * 2), this.transform.position.y, this.transform.position.z);
        else if (transform.rotation == Quaternion.Euler(player.faceLeft))
            return new Vector3(this.transform.position.x - (gridSize * 2), this.transform.position.y, this.transform.position.z);
        return new Vector3(0, 0, 0);
    }

}