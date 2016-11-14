using UnityEngine;
using System.Collections;

public class BulletSpawn : MonoBehaviour {
    [Header("Shot info")]
    [Tooltip("How many seconds between each shot")]
    public float shotRate = 2;
    [HideInInspector]
    public bool shieldRobotDead = false;
    [HideInInspector]
    public bool agileRobotDead = false;
    [Tooltip("Distance is in tile size, ie. if player is 4 tiles away shoot")]
    public float distanceToShoot = 4;
    [HideInInspector]
    public bool turnShootOn = false;
    public GameObject bullet;
    public GameObject turretParticles;

    [Header("Sound")]
    [Range(0, 1)]
    public float bulletShotVolume = 1;

    private AudioSource turretSource;

    private float timer;
    private GameObject agilePlayer;
    private GameObject shieldPlayer;
    private float gridSize = 1.25f;
    

	// Use this for initialization
	void Start () {

        agilePlayer = GameObject.FindWithTag("AgileRobot");
        shieldPlayer = GameObject.FindWithTag("ShieldRobot");

        turretSource = this.transform.GetComponent<AudioSource>();

        turretSource.loop = false;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
   
        if(shieldPlayer == null || agilePlayer == null)
        {
            return;
        }
        else if ((agilePlayer.transform.position - transform.position).magnitude < (distanceToShoot * gridSize) ||
                (shieldPlayer.transform.position - transform.position).magnitude < (distanceToShoot * gridSize) ||
                 turnShootOn == true)
        {
            if (timer >= shotRate)
            {             
                ShootBullet();
            }
        }
    }

    void ShootBullet()
    {
        turretParticles.GetComponent<ParticleSystem>().Emit(1);
        Instantiate(bullet, this.transform.position, this.transform.rotation);

        timer = 0;
        turretSource.PlayOneShot(turretSource.clip, bulletShotVolume);
    }

    public void SetAgileDead()
    {
        agileRobotDead = true;
    }
    public void SetShieldDead()
    {
        shieldRobotDead = true;
    }

}
