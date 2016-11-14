using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {
    
    public GameObject UIText;
    [Header("End Locations")]
    [Tooltip("Place the appropriate endlocations here")]
    public GameObject endLocationOne;
    public GameObject endLocationTwo;
    public GameObject gameCompleteButton;

    private GameObject myEventSystem;
    private AudioSource[] audioSources;
    [HideInInspector]
    public bool finishGame = false;

    private bool dontCallThisAgain = false;

    // Use this for initialization
    void Start () {
        myEventSystem = GameObject.Find("EventSystem");
        audioSources = GameObject.FindObjectsOfType<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        if (endLocationOne.GetComponent<EndLocationOne>().locationOneHasPlayer == true &&
           endLocationTwo.GetComponent<EndLocationTwo>().locationTwoHasPlayer == true &&
           dontCallThisAgain == false)
        {
            foreach(AudioSource source in audioSources)
            {
                source.enabled = false;
            }

            Finish();
        }	

        if(finishGame)
        {
            UIText.SetActive(true);
            
        }
	}

    public void Finish()
    {
        dontCallThisAgain = true;
        finishGame = true;
        StartCoroutine(AnotherHighlightButtonScript());

    }

    public IEnumerator AnotherHighlightButtonScript()
    {
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(gameCompleteButton);
        Time.timeScale = 0.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
