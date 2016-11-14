using UnityEngine;
using System.Collections;

public class MakeObjectsTransparent : MonoBehaviour {

    private GameObject gridManager;
    private GameObject[] allObjects;

	// Use this for initialization
	void Start () {
        gridManager = GameObject.FindWithTag("Game Overseer");
        allObjects = FindObjectsOfType<GameObject>();        
	}
	
	// Update is called once per frame
	void Update () {
        LayerMask playersLayer = gridManager.GetComponent<SwapCharacter>().currentPlayer.layer;

        foreach(GameObject obj in allObjects)
        {
            if (obj.layer != playersLayer)
            {
                //if (obj.CompareTag("Floor") || obj.CompareTag("Elevator") || obj.CompareTag("MovingPlatformX") || obj.CompareTag("MovingPlatformZ") || obj.CompareTag("Switch"))
                //{
                if(obj.CompareTag("Decal"))
                {
                    Color transparent = obj.GetComponentInChildren<Renderer>().material.color;
                    transparent.a = 0.3f;
                    obj.GetComponentInChildren<Renderer>().material.color = transparent;
                }
                    
                //}
            }             
            else
            {
                Color opaque = obj.GetComponentInChildren<Renderer>().material.color;
                opaque.a = 1;
                obj.GetComponentInChildren<Renderer>().material.color = opaque;
            }
                 
        }
	}
}
