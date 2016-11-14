using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// This code should be used with a trigger collider that the player can walk into and then hit a button to activate the bridge.
/// 
/// </summary>

public class Switch : MonoBehaviour {

    [Header("Bridge to activate")]
    [Tooltip("Bridge that should be associated to this switch, for PROTOTPYE THIS IS SET TO 'V'")]
    public GameObject bridge;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Box"))
        {
            bridge.GetComponent<MeshRenderer>().enabled = true;
            bridge.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
