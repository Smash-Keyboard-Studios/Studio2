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

    [Header("Build Indexes for the first scene of each Level")]
    public int Level1BuildIndex;
    public int Level2BuildIndex;

    [Header("Menu GameObject Prefabs")]
    [SerializeField] private GameObject levelSelectObj;
    [SerializeField] private GameObject gameMenuObj;
    [SerializeField] private GameObject optionsObj;
    [SerializeField] private GameObject controlsObj;


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

        //make sure all the menu prefabs are dontdestroyonload
        DontDestroyOnLoad(levelSelectObj);
        DontDestroyOnLoad(gameMenuObj);
        DontDestroyOnLoad(optionsObj);
        DontDestroyOnLoad(controlsObj); //none of these are working rn :(

        //set all menu objects to false initially through press return button
        PressReturn();
    }


    //subroutines that aren't button presses
    private void EnterMenu()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void EnterLevel()
    {
        Cursor.lockState = CursorLockMode.Confined;
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


    //level selection function
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

        levelSelectObj.SetActive(false);
        gameMenuObj.SetActive(false);
        optionsObj.SetActive(false);
        controlsObj.SetActive(false);

        if(wasInControls) { PressOptions(); } //reopen options if was in controls before

        if(wasInOptions) { PressGameMenu(); } //reopen game menu if was in options from game menu before
    }

    public void PressMainMenu()
    {
        EnterMenu();

        SceneManager.LoadScene(mainMenuBuildIndex);
    }
}
