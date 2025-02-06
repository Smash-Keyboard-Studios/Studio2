using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //stuff ensuring there is a single instance of the UI Manager
    [HideInInspector] public static UIManager Instance { get { return _instance; } }
    private static UIManager _instance;


    [Header("Build Indexes for scenes")]
    public int MainMenuBuildIndex;
    public int LevelSelectBuildIndex;
    public int OptionsBuildIndex;
    public int ControlsBuildIndex;
    public int CreditsBuildIndex;
    public int StartingLevelBuildIndex;
    [Header("Build Indexes for main menu and all levels")]
    public int[] LevelBuildIndexes; //contains all the build indexes that are a game level
    //^ this includes main menu as 0th

    [Header("UI Variables")]
    [SerializeField] private int CurrentLevelBuildIndex; //records whichever game level the player is currently in
    [SerializeField] private Vector3 CurrentPlayerPosition; //records current position of player


    private void Awake()
    {
        //makes sure there is only one UI Manager and that it is set to this
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // prevents this from being destroyed between scenes
        }

        //set tha variables
        //i am counting the main menu as a level so that if the game hasn't started
        //then you will return to the main menu from other scenes instead of starting the game
        CurrentLevelBuildIndex = MainMenuBuildIndex; //build index is set to main menu when the game hasn't started
        CurrentPlayerPosition = Vector3.zero;
    }

    private void Update()
    {
        OnSceneChange();
    }


    //subroutines that aren't button presses
    public void OnSceneChange() //this should run whenever there is a scene change
    {
        int newScene = SceneManager.GetActiveScene().buildIndex;
        if (LevelBuildIndexes.Contains(newScene))
        {
            CurrentLevelBuildIndex = newScene;
        }
    }

    private void EnterMenu()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;

        //////if can find a player in the scene, set current player position to players pos
    }

    private void EnterLevel()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }


    //main menu buttons
    public void PressStartGame()
    {
        EnterLevel();

        SceneManager.LoadScene(StartingLevelBuildIndex);
        OnSceneChange();
    }

    public void PressLevelSelect()
    {
        EnterMenu();

        SceneManager.LoadScene(LevelSelectBuildIndex);
    }

    public void PressOptions()
    {
        EnterMenu();

        SceneManager.LoadScene(OptionsBuildIndex);
    }

    public void PressControls()
    {
        EnterMenu();

        SceneManager.LoadScene(ControlsBuildIndex);
    }

    public void PressCredits()
    {
        EnterMenu();

        SceneManager.LoadScene(CreditsBuildIndex);
    }

    public void PressQuit()
    {
        Application.Quit();
    }


    //game menu buttons
    public void PressReturnToLevel()
    {
        if(CurrentLevelBuildIndex == MainMenuBuildIndex)
        {
            EnterMenu();
        }
        else
        {
            EnterLevel();
        }

        SceneManager.LoadScene(CurrentLevelBuildIndex);
        //////if can find a player in the scene, set players pos to current player position
    }

    public void PressMainMenu()
    {
        EnterMenu();

        SceneManager.LoadScene(MainMenuBuildIndex);
        OnSceneChange(); //build index is set to main menu when the game hasn't started
    }
}
