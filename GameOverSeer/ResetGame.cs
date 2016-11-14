using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ResetGame : MonoBehaviour {

    private GameObject myEventSystem;
    private AudioSource[] objectsWithSound;
    private Scene currentLevel;
	// Use this for initialization
	void Start () {
        currentLevel = SceneManager.GetActiveScene();
        objectsWithSound = GameObject.FindObjectsOfType<AudioSource>();
        myEventSystem = GameObject.Find("EventSystem");
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Pause") && GameObject.FindWithTag("Game Overseer").GetComponent<EndGame>().finishGame == false && this.transform.GetComponent<PauseUnPause>().paused == false)
        {
            foreach(AudioSource a in objectsWithSound)
            {
                a.Pause();
            }
            
            this.transform.gameObject.GetComponent<PauseUnPause>().PauseGame();
        }
        else if (Input.GetButtonDown("Pause") && GameObject.FindWithTag("Game Overseer").GetComponent<EndGame>().finishGame == false && this.transform.GetComponent<PauseUnPause>().paused == true)
        {
            foreach (AudioSource a in objectsWithSound)
            {
                a.UnPause();
            }
            this.transform.gameObject.GetComponent<PauseUnPause>().UnPauseGame();
        }
	
	}
}
