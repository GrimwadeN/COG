using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseUnPause : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject resumeButton;

    private GameObject myEventSystem;
    [HideInInspector]
    public bool paused = false;

    private AudioSource[] audioSources;

	// Use this for initialization
	void Start () {
        UnPauseGame();
        pauseMenu.SetActive(false);



        myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);

        audioSources = GameObject.FindObjectsOfType<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
        if(paused)
        {
            foreach(AudioSource source in audioSources)
            {
                if (source == null)
                    continue;      
                source.Pause();
                if(source.CompareTag("ShieldRobot") || source.CompareTag("AgileRobot"))
                {
                    source.Stop();
                }
            }
            pauseMenu.SetActive(true);
            
        }
        else if(!paused)
        {
            foreach (AudioSource source in audioSources)
            {
                if (source == null)
                    continue;

                source.UnPause();
            }
            pauseMenu.SetActive(false);
        }
	}

    public void PauseGame()
    {
        StartCoroutine(HighlightMyDamnButton());     
        paused = true;

    }

    public void UnPauseGame()
    {
        paused = false;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.2f * Time.timeScale;
    }

    public IEnumerator HighlightMyDamnButton()
    {

        myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(resumeButton);
        Time.timeScale = 0.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    
}
