using UnityEngine;
using System.Collections;

public class BridgeSetInactive : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
	}
	

}
