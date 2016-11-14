/// **********************************
/// Changed script so that entering these acts as a toggle to move
/// instead of having them constantly moving.
/// **********************************


using UnityEngine;
using System.Collections;

public class MakeThingsMove : MonoBehaviour {

    [Header("Object Linked To This")]
    [Tooltip("Drag whichever object you want to move when this is activated to here")]
    public GameObject[] objects;

    [Header("Animation/ Particle Control")]
    public Animator mandyAnim;
    public Animator switchAnim;
    public float delay;
    public ParticleSystem switchParticles;

    [Header("Sound")]
    public AudioClip pressurePadSound;
    public AudioClip switchSound;

    [Header("Sound Volume")]
    [Range(0, 1)]
    public float pressurePadVolume = 1;
    [Range(0, 1)]
    public float switchVolume = 1;

    private AudioSource pressureSource;
    private AudioSource switchSource;

    private bool activateObject = false;
    private bool activateObject2 = false;
    private bool canUseSwitch = true;
#pragma warning disable CS0414 // The field 'MakeThingsMove.autoOrNotAuto' is assigned but its value is never used
    private bool autoOrNotAuto = false;
#pragma warning restore CS0414 // The field 'MakeThingsMove.autoOrNotAuto' is assigned but its value is never used
    private SwapCharacter currentCharacter;

    void Start()
    {
        currentCharacter = GameObject.FindWithTag("Game Overseer").GetComponent<SwapCharacter>();

        if (this.transform.CompareTag("PressurePlate"))
            pressureSource = this.transform.GetComponent<AudioSource>();
        else if (this.transform.CompareTag("Switch"))
            switchSource = this.transform.GetComponent<AudioSource>();


    }

    void Update()
    {

        if (activateObject == true)
        { 
            // if the agile robot hits V while on the switch turn things on/off or change their state auto/not auto
            if (Input.GetButtonDown("Fire2") && currentCharacter.currentPlayer.CompareTag("AgileRobot"))
            {
                if(currentCharacter.currentPlayer.GetComponent<PlayerGridMovement>().playerState == PlayerGridMovement.PLAYERSTATE.IDLE && canUseSwitch == true)
                {                    
                    canUseSwitch = false;
                    foreach (GameObject obj in objects)
                    {
                        currentCharacter.currentPlayer.GetComponent<PlayerGridMovement>().mandyInteracting = true;
                        StartCoroutine(Delay(delay, obj));
                    }
                } 
            }
        }
        if(activateObject2 == true)
        {

            foreach (GameObject obj in objects)
            {
                if (obj.CompareTag("Elevator") || obj.CompareTag("MovingPlatformX") || obj.CompareTag("MovingPlatformZ"))
                {
                    obj.GetComponent<MoveAllPlatformsGridBased>().activated = true;
                    activateObject2 = false;
                }
                else if (obj.CompareTag("Turret"))
                    obj.GetComponentInChildren<BulletSpawn>().turnShootOn = true;
                else if (obj.CompareTag("Bridge"))
                    obj.gameObject.SetActive(true);
                else if (obj.CompareTag("Wall"))
                    if (obj.activeSelf == true)
                        obj.SetActive(false);
                    else
                        obj.SetActive(true);
            }
            
        }
    }


    public IEnumerator Delay(float seconds, GameObject obj)
    {
        mandyAnim.SetTrigger("mandySwitch");
        switchParticles.Play();
        switchSource.PlayOneShot(switchSound, switchVolume);
        

        yield return new WaitForSecondsRealtime(seconds * 0.5f);
        switchParticles.Play();
        yield return new WaitForSecondsRealtime(seconds * 0.5f);
        
        canUseSwitch = true;
        if (obj.CompareTag("Elevator") || obj.CompareTag("MovingPlatformX") || obj.CompareTag("MovingPlatformZ"))
        {
            // swap color of lights on switch
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Glow"))
                {
                    Animator anim = child.GetComponent<Animator>();
                    anim.SetTrigger("Swap");
                }
            }

            if (obj.GetComponent<MoveAllPlatformsGridBased>().activated == false)
                obj.GetComponent<MoveAllPlatformsGridBased>().activated = true;
            else
                obj.GetComponent<MoveAllPlatformsGridBased>().activated = false;
            obj.GetComponent<MoveAllPlatformsGridBased>().platformState = MoveAllPlatformsGridBased.PLATFORMSTATE.MOVING;

            currentCharacter.currentPlayer.GetComponent<PlayerGridMovement>().mandyInteracting = false;
            activateObject = false;

        }
        else if (obj.CompareTag("Turret"))
            obj.GetComponentInChildren<BulletSpawn>().turnShootOn = true;
        else if (obj.CompareTag("Bridge"))
            obj.gameObject.SetActive(true);
        else if (obj.CompareTag("Wall"))
            if (obj.activeSelf == true)
                obj.SetActive(false);
            else
                obj.SetActive(true);

        
    }

    void OnTriggerEnter(Collider other)
    {
        // if a box or shield robot is in the pressure plate collider set things to be active/inactive
        if (other.transform.CompareTag("Box") && this.transform.CompareTag("PressurePlate") ||
                 other.transform.CompareTag("ShieldRobot") && this.transform.CompareTag("PressurePlate"))
        {
            this.transform.parent.GetComponent<Animator>().SetBool("ColorSwap", true);
            pressureSource.PlayOneShot(pressurePadSound, pressurePadVolume);
            foreach (GameObject obj in objects)
            {
                if (obj.CompareTag("Elevator") || obj.CompareTag("MovingPlatformX") || obj.CompareTag("MovingPlatformZ"))
                {
                    obj.GetComponent<MoveAllPlatformsGridBased>().lerpMoveTime = 0;
                    obj.GetComponent<MoveAllPlatformsGridBased>().activated = true;
                    obj.GetComponent<MoveAllPlatformsGridBased>().platformState = MoveAllPlatformsGridBased.PLATFORMSTATE.MOVING;
                }
                else if (obj.CompareTag("Turret"))
                    obj.GetComponentInChildren<BulletSpawn>().turnShootOn = true;
                else if (obj.CompareTag("Wall"))
                    obj.SetActive(false);
            }
            activateObject2 = true;
        }
    }
    void OnTriggerStay(Collider other)
    {
        // turn light on if its mandy
        if(other.CompareTag("AgileRobot"))
            if(this.transform.CompareTag("Switch"))
            {
                activateObject = true;
                switchAnim.SetBool("openSwitch", true);
            }
    }

    void OnTriggerExit(Collider other)
    {
        // turn light on if its mandy
        if (other.CompareTag("AgileRobot"))
        {
            if (this.transform.CompareTag("Switch"))
            {
                switchAnim.SetBool("openSwitch", false);
            }
        }

        // if a box or shield robot is in the pressure plate collider set things to be active/inactive
        else if (other.transform.CompareTag("Box") && this.transform.CompareTag("PressurePlate") ||
                 other.transform.CompareTag("ShieldRobot") && this.transform.CompareTag("PressurePlate"))
        {
            Debug.Log("Left Pad");
            this.transform.parent.GetComponent<Animator>().SetBool("ColorSwap", false);
            foreach (GameObject obj in objects)
            {
                if (obj.CompareTag("Elevator") || obj.CompareTag("MovingPlatformX") || obj.CompareTag("MovingPlatformZ"))
                {
                    obj.GetComponent<MoveAllPlatformsGridBased>().platformState = MoveAllPlatformsGridBased.PLATFORMSTATE.MOVING;
                    obj.GetComponent<MoveAllPlatformsGridBased>().activated = false;
                    obj.GetComponent<MoveAllPlatformsGridBased>().lerpMoveTime = 0;
                    
                }
                else if (obj.CompareTag("Wall"))
                    obj.SetActive(true);
            }
        }
        activateObject = false;
        activateObject2 = false;
    }

  

}
