using UnityEngine;
using System.Collections;

public class GridRegisterForMovingObjects : MonoBehaviour {

    public bool moveable = false;
    private GridManager grid = null;
    private Vector3 lastPos = Vector3.zero;

    

	// Use this for initialization
	void Start () {
        grid = GameObject.FindWithTag("Game Overseer").GetComponent<GridManager>();
        lastPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
        if(moveable)
        {
            if(this.transform.position != lastPos)
            {
                grid.UnRegisterGameObject( this.gameObject);
                grid.RegisterGameObject(this.gameObject);
                lastPos = this.transform.position;
            }
        }
	}
}
