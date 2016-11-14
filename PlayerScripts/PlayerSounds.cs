using UnityEngine;
using System.Collections;

public class PlayerSounds : MonoBehaviour
{

    // billy's sounds
    public AudioClip idleBilly;
    public AudioClip movingBilly;
    // mandy's sounds
    public AudioClip idleMandy;
    public AudioClip movingMandy;
    // Audio sources
    private AudioSource billySound;
    private AudioSource mandySound;
    // current robot being used
    private SwapCharacter currentCharacter;

    // Use this for initialization
    void Start()
    {
        billySound = GameObject.FindWithTag("ShieldRobot").GetComponent<AudioSource>();
        mandySound = GameObject.FindWithTag("AgileRobot").GetComponent<AudioSource>();
        currentCharacter = GameObject.FindWithTag("Game Overseer").GetComponent<SwapCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCharacter.currentPlayer.CompareTag("ShieldRobot"))
        {
            if (this.transform.GetComponent<PlayerGridMovement>().playerState == PlayerGridMovement.PLAYERSTATE.MOVING)
            {
                if (billySound.clip != movingBilly)
                {
                    Debug.Log("Am I being called to much?");
                    BillyMovingSoundPlay();
                }
            }
            else if (this.transform.GetComponent<PlayerGridMovement>().playerState == PlayerGridMovement.PLAYERSTATE.IDLE)
            {
                if (billySound.clip != idleBilly)
                {
                    Debug.Log("Is idle being called to much?");
                    BillyIdleSoundPlay();
                }

            }
            
        }

        //if (!billySound.isPlaying)
        //{
        //    billySound.Play();
        //}

        

        //// Mandy's sounds
        //if (currentCharacter.currentPlayer.CompareTag("AgileRobot"))
        //{
        //    if (this.transform.GetComponent<PlayerGridMovement>().playerState == PlayerGridMovement.PLAYERSTATE.IDLE)
        //    {
        //        mandySound.volume = 1;
        //        mandySound.clip = idleMandy;
        //        mandySound.loop = true;
        //        if (!mandySound.isPlaying)
        //            mandySound.Play();
        //    }

        //    if (this.transform.GetComponent<PlayerGridMovement>().playerState == PlayerGridMovement.PLAYERSTATE.MOVING)
        //    {
        //        mandySound.loop = false;
        //        mandySound.clip = movingMandy;
        //        if (!mandySound.isPlaying)
        //            mandySound.Play();
        //    }
        //}
        //else
        //    mandySound.Stop();
    }

    void BillyIdleSoundPlay()
    {
        billySound.Stop();
        billySound.loop = true;
        billySound.clip = idleBilly;
        billySound.Play();
    }

    void BillyMovingSoundPlay()
    {
        billySound.Stop();
        billySound.loop = true;
        
        billySound.clip = movingBilly;
        billySound.Play();
    }
}
