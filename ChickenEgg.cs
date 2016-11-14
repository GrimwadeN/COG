using UnityEngine;
using System.Collections;

public class ChickenEgg : MonoBehaviour {
    [SerializeField]
#pragma warning disable CS0649 // Field 'ChickenEgg.anim' is never assigned to, and will always have its default value null
    Animator anim;
#pragma warning restore CS0649 // Field 'ChickenEgg.anim' is never assigned to, and will always have its default value null

    void OnTriggerEnter(Collider other)
    {
        if((other.CompareTag("ShieldRobot") || other.CompareTag("AgileRobot")))
        {
            anim.SetBool("Play", true);
        }
    }
}
