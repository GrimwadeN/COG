using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// Default starting state for the bridge before switches are used activating/deactivating to solve the puzzle.
/// 
/// </summary>

public class BridgeSetInactive : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
	}
	

}
