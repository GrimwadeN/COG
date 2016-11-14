using UnityEngine;
using System.Collections;

public class EndLocationOne : MonoBehaviour {

    [HideInInspector]
    public bool locationOneHasPlayer = false;

    [Header("Sound")]
    public AudioClip endPointOneSound;
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
                endPointSource.PlayOneShot(endPointOneSound, endPointVolume);
                hasPlayedSound = true;
            }
            
            locationOneHasPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("AgileRobot") || other.CompareTag("ShieldRobot"))
        {
            hasPlayedSound = false;
            locationOneHasPlayer = false;
        }
    }
}
