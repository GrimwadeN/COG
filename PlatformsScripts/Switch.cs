using UnityEngine;
using System.Collections;

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
