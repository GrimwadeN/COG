using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Tile
{
    public List<GameObject> objects = new List<GameObject>();
}

public class GridManager : MonoBehaviour
{
    private Dictionary<int, Dictionary<int, Dictionary<int, Tile>>> gridTiles;
    private Dictionary<GameObject, Tile> gameObjectTileLookup;

    // HACK: double check gridSize. This may need to be adjusted if its not encompassing stuff properly
    private float gridSize = 1.25f;

    private GameObject[] allFloorTiles;
    private GameObject[] allBoxes;
    private GameObject[] allTurrets;
    private GameObject[] allWalls;
    private GameObject[] allElevators;
    private GameObject[] allBridges;
    private GameObject[] allPlatformsX;
    private GameObject[] allPlatformsZ;
    private GameObject[] allSwitches;
    private GameObject playerObj;
    private GameObject shieldObj;

    void Start()
    {
        playerObj = GameObject.FindWithTag("AgileRobot");
        shieldObj = GameObject.FindWithTag("ShieldRobot");
        gameObjectTileLookup = new Dictionary<GameObject, Tile>();

        allFloorTiles = GameObject.FindGameObjectsWithTag("Floor");
        allBoxes = GameObject.FindGameObjectsWithTag("Box");
        allTurrets = GameObject.FindGameObjectsWithTag("Turret");
        allWalls = GameObject.FindGameObjectsWithTag("Wall");
        allElevators = GameObject.FindGameObjectsWithTag("Elevator");
        allBridges = GameObject.FindGameObjectsWithTag("Bridge");
        allPlatformsX = GameObject.FindGameObjectsWithTag("MovingPlatformX");
        allPlatformsZ = GameObject.FindGameObjectsWithTag("MovingPlatformZ");
        allSwitches = GameObject.FindGameObjectsWithTag("Switch");


        foreach (GameObject floor in allFloorTiles)
            RegisterGameObject(floor);
        
        foreach (GameObject box in allBoxes)         
            RegisterGameObject(box);
        
        foreach (GameObject turret in allTurrets)
            RegisterGameObject(turret);
        
        foreach (GameObject wall in allWalls)
            RegisterGameObject(wall);

        foreach (GameObject elevator in allElevators)
            RegisterGameObject(elevator);

        foreach (GameObject bridge in allBridges)
            RegisterGameObject(bridge);

        foreach (GameObject platformX in allPlatformsX)
            RegisterGameObject(platformX);

        foreach (GameObject platformZ in allPlatformsZ)
            RegisterGameObject(platformZ);

        foreach (GameObject switches in allSwitches)
            RegisterGameObject(switches);

        RegisterGameObject(playerObj);
        RegisterGameObject(shieldObj);
    }

    public void SnapGridObjects()
    {
        List<GameObject> objects = new List<GameObject>();
        objects.Add( GameObject.FindWithTag("AgileRobot") );
        objects.Add( GameObject.FindWithTag("ShieldRobot") );
        objects.AddRange( GameObject.FindGameObjectsWithTag("Floor"));
        objects.AddRange( GameObject.FindGameObjectsWithTag("Box"));
        objects.AddRange( GameObject.FindGameObjectsWithTag("Turret"));
        objects.AddRange( GameObject.FindGameObjectsWithTag("Wall"));
        objects.AddRange( GameObject.FindGameObjectsWithTag("Elevator"));
        objects.AddRange( GameObject.FindGameObjectsWithTag("Bridge"));
        objects.AddRange( GameObject.FindGameObjectsWithTag("MovingPlatformX"));
        objects.AddRange( GameObject.FindGameObjectsWithTag("MovingPlatformZ"));


        float hs = gridSize * 0.5f;
        foreach ( var obj in objects)
        {
            if (obj == null)
                continue;

            Vector3 index = PosToIndex(obj.transform.position);
            Vector3 pos = IndexToPos(index) + new Vector3(hs, hs, hs);
            obj.transform.position = pos;
        }

    }

    public void RegisterGameObject(GameObject obj)
    {
        // Function to register gameobjects in a list within a tile space
        Tile tile = GetGridTileAtPosition(obj.transform.position);
        gameObjectTileLookup[obj] = tile;
        tile.objects.Add(obj);
    }

    public void UnRegisterGameObject(GameObject obj)
    {
        // function to remove from the list
        var tile = gameObjectTileLookup[obj];
        if( tile != null )
        {
            tile.objects.Remove(obj);
            gameObjectTileLookup[obj] = null;
        }   
    }

    public Tile GetGridTileAtPosition(Vector3 position)
    {
        // function to find a specific grid so what objects are in it can be checked
        Vector3 index = PosToIndex(position);
        return GetGridTileAtIndex((int)index.x, (int)index.y, (int)index.z);
    }

    public Tile GetGridTileAtIndex(int xIndex, int yIndex, int zIndex)
    {

        // returns which tile is at a specified grid
        if (gridTiles == null)
            gridTiles = new Dictionary<int, Dictionary<int, Dictionary<int, Tile>>>();

        if (!gridTiles.ContainsKey(xIndex))
            gridTiles[xIndex] = new Dictionary<int, Dictionary<int, Tile>>();

        if (!gridTiles[xIndex].ContainsKey(yIndex))
            gridTiles[xIndex][yIndex] = new Dictionary<int, Tile>();

        if (!gridTiles[xIndex][yIndex].ContainsKey(zIndex))
            gridTiles[xIndex][yIndex][zIndex] = new Tile();

        return gridTiles[xIndex][yIndex][zIndex];
    }
    public Vector3 PosToIndex(Vector3 position)
    {
        float hGridSize = gridSize * 0.5f;

        // define the tile size (1.25 x 1.25 x 1.25 cube)
        int xIndex = (int)(position.x / gridSize);
        int yIndex = (int)(position.y / gridSize);
        int zIndex = (int)(position.z / gridSize);

        if (position.x < 0) xIndex -= 1;
        if (position.y < 0) yIndex -= 1;
        if (position.z < 0) zIndex -= 1;

        return new Vector3(xIndex, yIndex, zIndex);
    }

    public Vector3 IndexToPos(Vector3 index)
    {
        // returns the index number
        if (index.x < 0) index.x += 1;
        if (index.y < 0) index.y += 1;
        if (index.z < 0) index.z += 1;

         return new Vector3((index.x * gridSize), index.y * gridSize, index.z * gridSize);
    }

    public bool TileHasObjectWithTag(string tag, Vector3 index)
    {
        var objects = GetGridTileAtIndex((int)index.x, (int)index.y, (int)index.z).objects;
        foreach (GameObject obj in objects)
            if (obj.tag == tag)
                return true;
        return false;
    }
    public bool TileHasObjectWithTag(string[] tag, Vector3 index)
    {
        var objects = GetGridTileAtIndex((int)index.x, (int)index.y, (int)index.z).objects;
        foreach (GameObject obj in objects)
            if (tag.Any( z => z == obj.tag))
                return true;
        return false;
    }

    public GameObject GameObjectOnTileAtIndex(string tag, Vector3 index)
    {
        var objects = GetGridTileAtIndex((int)index.x, (int)index.y, (int)index.z).objects;
        foreach (GameObject obj in objects)
            if (obj.tag == tag)
                return obj;
        return null;
    }

    public GameObject GameObjectOnTileAtIndex(string[] tag, Vector3 index)
    {
        var objects = GetGridTileAtIndex((int)index.x, (int)index.y, (int)index.z).objects;
        foreach (GameObject obj in objects)
            if( tag.Any( z => z == obj.tag) )
                return obj;
        return null;
    }

    void OnDrawGizmos()
    {
        float hGridSize = gridSize * 0.5f;

        if (gridTiles == null)
            return;

        foreach (var x_kv in gridTiles)
        {
            
            int xIndex = x_kv.Key;

            foreach (var y_kv in x_kv.Value)
            {
                int yIndex = y_kv.Key;

                foreach (var z_kv in y_kv.Value)
                {
                    int zIndex = z_kv.Key;

                    var tile = z_kv.Value;

                    if( tile.objects.Count > 0 )
                    {


                        Vector3 tilePos = new Vector3(
                      (xIndex * gridSize) + hGridSize,
                      (yIndex * gridSize) + hGridSize,
                      (zIndex * gridSize) + hGridSize);

                        Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        foreach (var obj in tile.objects)
                        {
                            Gizmos.DrawCube(obj.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
                        }

                        Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        Gizmos.DrawWireCube(tilePos, new Vector3(gridSize, gridSize, gridSize));
                    }   
                }
            }
        }
    }
}
