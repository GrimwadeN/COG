using UnityEngine;
using System.Collections;

public class EndLocationTwo : MonoBehaviour {

    [HideInInspector]
    public bool locationTwoHasPlayer = false;

    [Header("Sound")]
    public AudioClip endPointTwoSound;
    [Range(0, 1)]
    public float endPointVolume = 1;
    private bool hasPlayedSound = false;

    private AudioSource endPointSource;

    void Start()
    {
        endPointSource = GetComponent<AudioSource>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("AgileRobot") || other.CompareTag("ShieldRobot"))
        {
            if(hasPlayedSound == false)
            {
                endPointSource.PlayOneShot(endPointTwoSound, endPointVolume);
                hasPlayedSound = true;
            }
            
            locationTwoHasPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AgileRobot") || other.CompareTag("ShieldRobot"))
        {
            hasPlayedSound = false;
            locationTwoHasPlayer = false;
        }
    }
}
