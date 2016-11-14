using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHitWallReset : MonoBehaviour {

    public GameObject robotDeadUI;

    private Scene currentScene;
    private float timer = 0;
    private bool startDelay = false;

    private GameObject billy;
    private GameObject mandy;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        billy = GameObject.FindWithTag("ShieldRobot");
        mandy = GameObject.FindWithTag("AgileRobot");
    }

    void Update()
    {
        if(startDelay)
        {
            timer += Time.deltaTime;
            // Reset game after 2 seconds of robot dying
            if(timer >= 2) SceneManager.LoadScene(currentScene.name);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        // check which robot hit the wall and died, display ui
        if (other.CompareTag("ShieldRobot"))
        {
            billy.SetActive(false);
            robotDeadUI.SetActive(true);
            startDelay = true;
        }
        if (other.CompareTag("AgileRobot"))
        {
            mandy.SetActive(false);
            robotDeadUI.SetActive(true);
            startDelay = true;
        }
    }

   
}
