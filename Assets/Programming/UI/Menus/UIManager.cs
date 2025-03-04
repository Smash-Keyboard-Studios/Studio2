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
        DontDestroyOnLoad(controlsObj);

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

        //open level select prefab
    }

    public void PressOptions()
    {
        EnterMenu();

        //open options prefab
    }

    public void PressControls()
    {
        EnterMenu();

        //open controls prefab
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

        //open game menu prefab
    }

    public void PressReturn()
    {
        //store whether in controls rn so can keep options open
        bool wasInControls = controlsObj.activeSelf;
        //store whether in options from gamemenu rn so can keep game menu open
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
