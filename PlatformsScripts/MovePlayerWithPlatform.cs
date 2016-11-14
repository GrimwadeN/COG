using UnityEngine;
using System.Collections;

public class MovePlayerWithPlatform : MonoBehaviour {


    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("AgileRobot") || other.CompareTag("ShieldRobot") || other.CompareTag("Box"))
        {
            other.transform.SetParent(this.transform, true);    
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AgileRobot") || other.CompareTag("ShieldRobot") || other.CompareTag("Box"))
        {
            if (other.transform.CompareTag("Box"))
            {
                other.transform.GetComponent<BoxMove>().moveSpeed = 6;
                other.transform.SetParent(null);
                if (this.GetComponent<BoxMove>().boxState == BoxMove.BOXSTATE.FALLING)
                {
                    other.GetComponent<BoxMove>().boxState = BoxMove.BOXSTATE.FALLING;
                }
            }
            else
                other.transform.SetParent(null);
        }
    }
}
