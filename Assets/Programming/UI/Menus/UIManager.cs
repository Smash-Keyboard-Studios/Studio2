using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
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
    public bool inGameMenu;
    public bool inDialogueMenu;

    [Header("Tootips")]
    [SerializeField] private TextMeshProUGUI toolTipName;
    public string[] ToolTipNames; //The name of the tooltips. For example this would be: "How to Sprint!"
    [SerializeField] private TextMeshProUGUI toolTipText;
    public string[] ToolTipText;//The text of the tooltips. For example this would be: "Press shift!"
                                // now change the tooltips to the ones in the array.?// 
                                //on game menu press get random number between 0 and length of tooltipnames-1 (the -1 is because array indexing starts at 0)
                                //set tooltip name to tooltipnames(randomnumber)
                                //set tooltip text to tooltiptexts(randomnumber)


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
        inDialogueMenu = false;
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

        
        inGameMenu = false;
    }


    //main menu buttons
    public void PressPlay()
    {
        EnterMenu();

        levelSelectObj.SetActive(true);
    }

    public void PressOptions()
    {
        optionsObj.SetActive(true);
    }

    public void PressControls()
    {
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
        if (!controlsObj.activeInHierarchy && !optionsObj.activeInHierarchy)
        {
            EnterMenu();

            inGameMenu = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent <PlayerInput>().enabled = false;
            gameMenuObj.SetActive(true);
        }
    }


       

    public void PressReturn()
    {
        //store whether in controls rn so can keep options open
        bool wasInControls = controlsObj.activeInHierarchy && optionsObj.activeInHierarchy && !gameMenuObj.activeInHierarchy;
        //store whether in controls (from gamemenu) rn so can keep options open
        bool wasInControlsGameMenu = controlsObj.activeInHierarchy && optionsObj.activeInHierarchy && gameMenuObj.activeInHierarchy;
        //store whether in options (from gamemenu) rn so can keep game menu open
        bool wasInOptionsGameMenu = !controlsObj.activeInHierarchy && optionsObj.activeInHierarchy && gameMenuObj.activeInHierarchy;

        DisableAllMenus();

        if (wasInControls) { PressOptions(); } //reopen options if was in controls before
        else if (wasInControlsGameMenu) { PressGameMenu(); PressOptions(); } //reopen game menu and options if was in controls from game menu before
        else if(wasInOptionsGameMenu) { PressGameMenu(); } //reopen game menu if was in options from game menu before
        else { GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>().enabled = true; }
    }

    public void PressMainMenu()
    {
        EnterMenu();

        SceneManager.LoadScene(mainMenuBuildIndex);
    }
}
