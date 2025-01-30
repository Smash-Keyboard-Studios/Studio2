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


    [Header("Build Indexes for scenes - Please set to their indexes in the game build!!")]
    public int MainMenuBuildIndex;
    public int ControlsBuildIndex;
    public int CreditsBuildIndex;
    public int StartingLevelBuildIndex;

    [Header("UI Variables")]
    [SerializeField] private int CurrentLevelBuildIndex; //records whichever game level the player is currently in
    private int[] NonLevelBuildIndexes; //contains all the build indexes that aren't a game level


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
        NonLevelBuildIndexes = new int[2] { ControlsBuildIndex, CreditsBuildIndex };
    }

    private void Update()
    {
        OnSceneChange();
    }


    //subroutines that aren't button presses
    public void OnSceneChange() //this should run whenever there is a scene change
    {
        int newScene = SceneManager.GetActiveScene().buildIndex;
        if (!NonLevelBuildIndexes.Contains(newScene))
        {
            CurrentLevelBuildIndex = newScene;
        }
    }

    private void EnterMenu()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
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
        CurrentLevelBuildIndex = StartingLevelBuildIndex;
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
        EnterLevel();

        SceneManager.LoadScene(CurrentLevelBuildIndex);
    }

    public void PressMainMenu()
    {
        EnterMenu();

        SceneManager.LoadScene(MainMenuBuildIndex);
        CurrentLevelBuildIndex = MainMenuBuildIndex; //build index is set to main menu when the game hasn't started
    }
}
