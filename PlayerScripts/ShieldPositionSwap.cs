﻿using UnityEngine;
using System.Collections;

public class ShieldPositionSwap : MonoBehaviour {

    private bool isBlocking;

    [Header("Shield Positions")]
    [Tooltip("Drag an empty game object in this positions to have the shield collider be here")]
    public GameObject blockPosition;
    [Tooltip("Drag an empty game object in this positions to have the shield collider be here")]
    public GameObject platformPosition;

	// Use this for initialization
	void Start () {
        // shield starts off in blocking position   
        isBlocking = true;
        this.transform.position = blockPosition.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKeyDown(KeyCode.B))
        {
            if(isBlocking == true)
            {
                // if the shield is currently blocking swap to platform mode
                this.transform.position = platformPosition.transform.position;
                this.transform.Rotate(270, 0, 0);
                isBlocking = false;
            }
            else if(isBlocking == false)
            {
                // if platform mode swap to blocking mode
                this.transform.position = blockPosition.transform.position;
                this.transform.Rotate(270, 0, 0);
                isBlocking = true;
            }
        }
	}
}
