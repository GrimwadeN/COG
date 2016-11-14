using UnityEngine;
using System.Collections;

/// <summary>
///  This script is setup so that when a box is on the switch the bridge is active, when it is removed the bridge will turn off again.
/// </summary>

public class PressurePlateBridge : MonoBehaviour {

    [Header("Bridge to activate")]
    [Tooltip("Bridge that should be associated to this switch")]
    public GameObject bridge;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            bridge.GetComponent<MeshRenderer>().enabled = true;
            bridge.GetComponent<BoxCollider>().enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Box"))
        {
            bridge.GetComponent<MeshRenderer>().enabled = false;
            bridge.GetComponent<BoxCollider>().enabled = false;
        }
        
    }
}
