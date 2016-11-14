using UnityEngine;
using System.Collections;

public class ActivateUIInstuction : MonoBehaviour {

    public GameObject billyUI;
    public GameObject mandyUI;

    private GameObject gridManager;
    private float gridSize = 1.25f;
    private Vector3 gridAhead;
    private Vector3 myGrid;

	// Use this for initialization
	void Start () {

        gridManager = GameObject.FindWithTag("Game Overseer");
        billyUI.SetActive(false);
        mandyUI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        
        
        if (this.gameObject.CompareTag("ShieldRobot"))
        {
            BillyUIActivate();
        }
        else if(this.gameObject.CompareTag("AgileRobot"))
        {
            MandyUIActivate();
        }
	
	}

    void BillyUIActivate()
    {
        gridAhead = gridManager.GetComponent<GridManager>().PosToIndex(this.transform.position + (transform.forward * gridSize));
        if (gridManager.GetComponent<GridManager>().GameObjectOnTileAtIndex("Box", gridAhead) && gridManager.GetComponent<SwapCharacter>().currentPlayer.CompareTag("ShieldRobot"))
        {
            billyUI.SetActive(true);
        }
        else
        {
            billyUI.SetActive(false);
        }
    }

    void MandyUIActivate()
    {
        myGrid = gridManager.GetComponent<GridManager>().PosToIndex(this.transform.position);
        if (gridManager.GetComponent<GridManager>().GameObjectOnTileAtIndex("Switch", myGrid) && gridManager.GetComponent<SwapCharacter>().currentPlayer.CompareTag("AgileRobot"))
        {
            mandyUI.SetActive(true);
        }
        else
        {
            mandyUI.SetActive(false);
        }
    }
}
