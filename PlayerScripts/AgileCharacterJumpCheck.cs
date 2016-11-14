using UnityEngine;
using System.Collections;

public class AgileCharacterJumpCheck : MonoBehaviour
{
    [HideInInspector]
    public bool somethingHere = false;

    void OnTriggerStay(Collider other)
    {
        somethingHere = true;
    }
    void OnTriggerExit(Collider other)
    {
        somethingHere = false;
    }
}