using UnityEngine;
using System.Collections;

public class UIImageFaceCamera : MonoBehaviour {

    private GameObject cameraToLookAt;

    private Quaternion initRot;

    // Use this for initialization
    void Start () {
        cameraToLookAt = GameObject.FindWithTag("MainCamera");
        initRot = this.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        //this.transform.rotation = initRot;
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        // v.x = v.z = 0.0f;
        //transform.LookAt(cameraToLookAt.transform.position - v);
        transform.rotation = Camera.main.transform.rotation;
    }
}
