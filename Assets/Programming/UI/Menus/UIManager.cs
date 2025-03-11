using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //stuff ensuring there is a single instance of the UI Manager
    [HideInInspector] public static UIManager Instance { get { return instance; } }
    private static UIManager instance;


    [Header("Build Indexes for menu scenes")]
    [SerializeField] private int mainMenuBuildIndex;
    [SerializeField] private int creditsBuildIndex;

    [Header("Build Indexes for each Level")]
    public int[] Level1BuildIndexes;
    public int[] Level2BuildIndexes;

    [Header("Menu GameObject Prefabs")]
    [SerializeField] private GameObject levelSelectObj;
    [SerializeField] private GameObject gameMenuObj;
    [SerializeField] private GameObject optionsObj;
    [SerializeField] private GameObject controlsObj;
    //^ all these menu prefabs need to be dontdestroyonload to work properly


    private void Awake()
    {
        //makes sure there is only one UI Manager and that it is set to this
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // prevents this from being destroyed between scenes
        }
    }

    private void Start()
    {
        //set all menu objects to false initially through press return button
        //this is in the start function so that the menu objects can run their awake functions before being disabled
        DisableAllMenus();
    }


    //subroutines that aren't button presses
    private void EnterMenu()
    {
        Cursor.lockState = CursorLockMode.None;

        DisableAllMenus(); //makes sure the menus are all disabled after changin a scene
    }

    private void EnterLevel()
    {
        Cursor.lockState = CursorLockMode.Confined;

        DisableAllMenus(); //makes sure the menus are all disabled after changin a scene
    }

    private void DisableAllMenus()
    {
        levelSelectObj.SetActive(false);
        gameMenuObj.SetActive(false);
        optionsObj.SetActive(false);
        controlsObj.SetActive(false);
    }


    //main menu buttons
    public void PressPlay()
    {
        EnterMenu();

        levelSelectObj.SetActive(true);
    }

    public void PressOptions()
    {
        EnterMenu();

        optionsObj.SetActive(true);
    }

    public void PressControls()
    {
        EnterMenu();

        controlsObj.SetActive(true);
    }

    public void PressCredits()
    {
        EnterMenu();

        SceneManager.LoadScene(creditsBuildIndex);
    }

    public void PressQuit()
    {
        Application.Quit();
    }


    //level selection functions
    public void RestartLevel()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

        foreach(int index in Level1BuildIndexes)
        {
            if(index == currentBuildIndex) //if currently in level 1 scene
            {
                SelectLevel(Level1BuildIndexes[0]); //go to start of level 1
                return;
            }
        }

        foreach (int index in Level2BuildIndexes)
        {
            if (index == currentBuildIndex) //if currently in level 2 scene
            {
                SelectLevel(Level2BuildIndexes[0]); //go to start of level 2
                return;
            }
        }
    }

    public void SelectLevel(int BuildIndex)
    {
        EnterLevel();

        SceneManager.LoadScene(BuildIndex);
    }


    //game menu buttons
    public void PressGameMenu()
    {
        EnterMenu();

        gameMenuObj.SetActive(true);
    }

    public void PressReturn()
    {
        //store whether in controls rn so can keep options open
        bool wasInControls = controlsObj.activeSelf;
        //store whether in options (from gamemenu) rn so can keep game menu open
        bool wasInOptions = optionsObj.activeSelf && gameMenuObj.activeSelf;

        DisableAllMenus();

        if(wasInControls) { PressOptions(); } //reopen options if was in controls before

        if(wasInOptions) { PressGameMenu(); } //reopen game menu if was in options from game menu before
    }

    public void PressMainMenu()
    {
        EnterMenu();

        SceneManager.LoadScene(mainMenuBuildIndex);
    }
}
