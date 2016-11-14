using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuFunctions : MonoBehaviour
{

    public string SceneName;
    public GameObject menu;
    public GameObject playButton;
    public GameObject levelOne;
    public GameObject buttonMaster;
    public GameObject mainMenuButtons;
    private GameObject myEventSystem;
    
    private Vector3 menuStartPos;
    private Vector3 creditsPos;
    private Vector3 buttonPos;

    private bool creditMenuActive = false;

    private float lerpMoveTime = 0;
    private float moveSpeed = 1;

    private bool resetMenuPos = false;


    private MenuState menuState = MenuState.MAIN;

    private Vector3 startLerpPos;
    private Vector3 finishLerpPos;

    enum MenuState
    {
        MAIN,
        CREDITS,
        PLAY
    }

    void Start()
    {
        menuStartPos = menu.transform.position;
        creditsPos = new Vector3(menu.transform.position.x, menu.transform.position.y - Screen.height, menu.transform.position.z);
        myEventSystem = GameObject.Find("EventSystem");

        startLerpPos = menu.transform.position;
        finishLerpPos = menuStartPos;
        lerpMoveTime = 0;
        buttonPos = buttonMaster.transform.position;

        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.2f * Time.timeScale;
    }

    void Update()
    {
        switch (menuState)
        {
            case MenuState.MAIN:
                UpdateMenu();
                break;
            case MenuState.CREDITS:
                UpdateCredits();
                break;
            case MenuState.PLAY:
                UpdatePlay();
                break;
        }
    }


    void UpdatePlay()
    {
        lerpMoveTime += Time.deltaTime * moveSpeed;
        menu.transform.position = Vector3.Lerp(startLerpPos, finishLerpPos, lerpMoveTime);


        if (Input.GetButtonDown("Cancel"))
        {
            foreach (Transform child in mainMenuButtons.transform)
            {
                child.GetComponent<Button>().interactable = true;
            }
            StartCoroutine(ButtonActive());
            startLerpPos = menu.transform.position;
            finishLerpPos = menuStartPos;
            lerpMoveTime = 0;
            menuState = MenuState.MAIN;
        }
    }
    void UpdateMenu()
    {
        if(this.transform.position == finishLerpPos)
            buttonMaster.SetActive(false);

        lerpMoveTime += Time.deltaTime * moveSpeed;
        menu.transform.position = Vector3.Lerp(startLerpPos, finishLerpPos, lerpMoveTime);
    }
    
    void UpdateCredits()
    {      
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        lerpMoveTime += Time.deltaTime * moveSpeed;
        menu.transform.position = Vector3.Lerp(startLerpPos, finishLerpPos, lerpMoveTime);

        if (Input.GetButtonDown("Cancel"))
        {
            foreach (Transform child in mainMenuButtons.transform)
            {
                child.GetComponent<Button>().interactable = true;
            }
            menuState = MenuState.MAIN;
            StartCoroutine(ButtonActive());
            startLerpPos = menu.transform.position;
            finishLerpPos = menuStartPos;
            lerpMoveTime = 0;
        }
    }

    IEnumerator ButtonActive()
    {
        // when the menu resets make the play button the active button
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        yield return new WaitForSeconds(0.01f);
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(playButton);
    }

    public void LoadLevel()
    {
        SceneManager.UnloadScene(SceneManager.GetActiveScene());
        SceneManager.LoadScene(SceneName);
    }

    public void ExitGame()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadScene(currentScene.name);
        Application.Quit();
    }

    public void LoadPlay()
    {
        startLerpPos = menu.transform.position;
        finishLerpPos = new Vector3(menuStartPos.x, menuStartPos.y + Screen.height, menuStartPos.z);
        lerpMoveTime = 0;
        StartCoroutine(LevelSelectButton());
        foreach(Transform child in mainMenuButtons.transform)
        {
            child.GetComponent<Button>().interactable = false;
        }
        buttonMaster.SetActive(true);
        menuState = MenuState.PLAY;
    }
    public void LoadCredits()
    {
        foreach (Transform child in mainMenuButtons.transform)
        {
            child.GetComponent<Button>().interactable = false;
        }
        startLerpPos = menu.transform.position;
        finishLerpPos = creditsPos;
        lerpMoveTime = 0;
        menuState = MenuState.CREDITS;
    }

    IEnumerator LevelSelectButton()
    {
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        yield return new WaitForSeconds(0.01f);
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(levelOne);
    }
}
