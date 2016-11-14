using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlayButtonMoveStuff : MonoBehaviour {

    public GameObject textGroup;
    public GameObject levelGroup;
    public GameObject levelOne;
    public GameObject startPos;
    public GameObject endPos;   
    public float moveSpeed = 1;

    private bool move = false;
    private bool moveBack = false;
    private float lerpMoveTime;
    private float screenWidth;
    private GameObject myEventSystem;
    private GameObject playButton;
    private GameObject exitButton;
    private Vector3 startLerpPos;
    private Vector3 foregroundLerpStart;
    private Vector3 foregroundTarget;
    private Vector3 target;
    private Vector3 newStartPos;
    private Vector3 newForeStartPos;
    private Vector3 newForeTargetPos;
    

    // Use this for initialization
    void Start () {
        move = false;
        moveBack = false;
        screenWidth = Screen.width;

        startLerpPos = textGroup.transform.GetComponent<RectTransform>().position;
        target = new Vector3(startLerpPos.x - (screenWidth / 2), startLerpPos.y, startLerpPos.z);

        foregroundLerpStart = levelGroup.transform.GetComponent<RectTransform>().position;
        foregroundTarget = new Vector3(foregroundLerpStart.x, endPos.transform.position.y, foregroundLerpStart.z);

        myEventSystem = GameObject.Find("EventSystem");

        playButton = GameObject.Find("Play");
        exitButton = GameObject.Find("Exit");

        levelGroup.transform.GetChild(0).gameObject.SetActive(false);


        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.2f * Time.timeScale;

    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetButtonDown("Cancel"))
        {
            MoveBack();
        }

        if (move)
        {
            lerpMoveTime += Time.deltaTime * moveSpeed;
            levelGroup.transform.GetChild(0).gameObject.SetActive(true);
            // move to target position
            textGroup.GetComponent<RectTransform>().position = Vector3.Lerp(startLerpPos, target, lerpMoveTime);
            levelGroup.GetComponent<RectTransform>().position = Vector3.Lerp(foregroundLerpStart, foregroundTarget, lerpMoveTime);

            

            // swap back to idle state to allow next move
            if (lerpMoveTime >= 1.0f)
            {
                playButton.GetComponent<Button>().interactable = false;
                exitButton.GetComponent<Button>().interactable = false;
                move = false;
                lerpMoveTime = 0.0f;       
            }
        }

        if(moveBack)
        {
            lerpMoveTime += Time.deltaTime * moveSpeed;

            // move to target position
            textGroup.GetComponent<RectTransform>().position = Vector3.Lerp(newStartPos, target, lerpMoveTime);
            levelGroup.GetComponent<RectTransform>().position = Vector3.Lerp(newForeStartPos, startPos.transform.position, lerpMoveTime);

            

            if (lerpMoveTime >= 1.0f)
            {
                playButton.GetComponent<Button>().interactable = true;
                exitButton.GetComponent<Button>().interactable = true;
                levelGroup.transform.GetChild(0).gameObject.SetActive(false);

                moveBack = false;
                lerpMoveTime = 0.0f;
            }
        }

        
    }

    public void MoveStuff()
    {
        move = true;
        target = new Vector3(startLerpPos.x - (screenWidth / 2), startLerpPos.y, startLerpPos.z);
        StartCoroutine(LevelSelectButton());
    }

    public void MoveBack()
    {
        moveBack = true;
        move = false;
        StartCoroutine(MenuSelectButton());
        newStartPos = textGroup.transform.position;
        target = startLerpPos;
        newForeStartPos = levelGroup.transform.position;
        newForeTargetPos = foregroundLerpStart;
    }

    public IEnumerator LevelSelectButton()
    {
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(levelOne);
    }

    public IEnumerator MenuSelectButton()
    {
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(playButton);
    }
}
