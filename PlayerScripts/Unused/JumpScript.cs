using UnityEngine;
using System.Collections;


/// <summary>
/// 
/// Jump needs to be fixed to stop the jittering
/// 
/// </summary>
public class JumpScript : MonoBehaviour {

    public float jumpHeight = 15;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.transform.position += new Vector3(0.1f, 0, 0);
        }




        
    }

}
