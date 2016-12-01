using UnityEngine;
using System.Collections;

public class ChickenEgg : MonoBehaviour {
    [SerializeField]
    Animator anim;


    void OnTriggerEnter(Collider other)
    {
        if((other.CompareTag("ShieldRobot") || other.CompareTag("AgileRobot")))
        {
            anim.SetBool("Play", true);
        }
    }
}
