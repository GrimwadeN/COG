using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Bullet : MonoBehaviour {
    [Header("Bullet Info")]
    [Tooltip("Bullets move speed")]
    public float speed = 200;
    [Tooltip("Time till bullet is removed from the scene if it hits nothing")]
    public float bulletLife = 10;

    [Header("Sound Clips")]
    public AudioClip bulletHitMandy;
    public AudioClip bulletHitBilly;    

    [Header("Sound Volume")]
    [Range(0, 1)]
    public float bulletHittingMandyVolume = 1;
    [Range(0, 1)]
    public float bulletHittingBillyVolume = 1;

    [Header("Particles")]
    public GameObject smoke;
    public GameObject explosion;
    

    private AudioSource bulletSource;

    private GridManager gridManager;
    private float destroyBulletTimer = 0.0f;
    private GameObject billy;
    private GameObject mandy;
    private GameObject[] boxes;
    private List<GameObject> bulletBlockers = new List<GameObject>();
    void Start()
    {
        gridManager = GameObject.FindWithTag("Game Overseer").GetComponent<GridManager>();
        billy = GameObject.FindWithTag("ShieldRobot");
        mandy = GameObject.FindWithTag("AgileRobot");
        boxes = GameObject.FindGameObjectsWithTag("Box");

        bulletBlockers.Add(billy);       
        foreach (GameObject box in boxes)
            bulletBlockers.Add(box);

        transform.Rotate(new Vector3(270, 0, 0));

        bulletSource = this.transform.GetComponent<AudioSource>();

        bulletSource.loop = false;
        bulletSource.playOnAwake = false;
    }
	
	// Update is called once per frame
	void Update () {

        transform.position += transform.up * speed * Time.deltaTime;

        destroyBulletTimer += Time.deltaTime;
        if (destroyBulletTimer >= bulletLife)
            Destroy(this.gameObject);
    }

    public IEnumerator BulletDeath(float seconds, GameObject robot)
    {
        if(robot.CompareTag("ShieldRobot"))
        {
            // play particle
            //SmokeParticle(robot);
            // disable bullet visuals
            this.transform.GetComponent<MeshRenderer>().enabled = false;
            this.transform.GetComponent<BoxCollider>().enabled = false;
            // play sound
            bulletSource.PlayOneShot(bulletHitBilly, bulletHittingBillyVolume);
            yield return new WaitForSecondsRealtime(seconds);
            Destroy(this.gameObject);
        }
        if (robot.CompareTag("AgileRobot"))
        {
            // play particle
            ExplosionParticle(robot);
            // reset mandy position           
            robot.GetComponent<PlayerGridMovement>().MandyReset();
            // make it so bullet can't be seen but can play sound
            this.transform.GetComponent<MeshRenderer>().enabled = false;
            this.transform.GetComponent<BoxCollider>().enabled = false;
            yield return new WaitForSeconds(seconds);
            // destroy bullet
            Destroy(this.gameObject);
        }
        if (robot.CompareTag("Box"))
        {
            // play particle
            //SmokeParticle(robot);
            // disable visuals and play sound
            this.transform.GetComponent<MeshRenderer>().enabled = false;
            this.transform.GetComponent<BoxCollider>().enabled = false;
            bulletSource.PlayOneShot(bulletHitBilly, bulletHittingBillyVolume);
            yield return new WaitForSecondsRealtime(seconds);
            Destroy(this.gameObject);
        }
    }

    void ExplosionParticle(GameObject robot)
    {
        StartCoroutine(KillParticle(robot));
    }


    void SmokeParticle(GameObject robot)
    {
        StartCoroutine(KillBullet(robot));

    }

    IEnumerator KillBullet(GameObject robot)
    {
        GameObject particle = Instantiate(Resources.Load("Hit_Smoke_V2", typeof(GameObject)) as GameObject);
        particle.transform.position = robot.transform.position;
        particle.GetComponent<ParticleSystem>().Emit(1);
        yield return new WaitForSeconds(2);
        Destroy(particle);
    }
    IEnumerator KillParticle(GameObject robot)
    {
        Debug.Log("got into coroutine");
        GameObject particle = Instantiate(Resources.Load("Bullet_Hit_Spark_Sparkles", typeof(GameObject)) as GameObject);
        particle.transform.position = robot.transform.position;
        particle.GetComponent<ParticleSystem>().Emit(1);
        yield return new WaitForSeconds(2);
        Destroy(particle);
    }

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(BulletDeath(1, other.gameObject));

    }

    
}
