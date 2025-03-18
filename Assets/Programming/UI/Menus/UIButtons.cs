using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    //public functions that reference UI manager for the buttons in scenes to use
    public void PressReturn()
    {
        UIManager.Instance.PressReturn();
    }

    public void PressMainMenu()
    {
        UIManager.Instance.PressMainMenu();
    }

    public void PressPlay()
    {
        UIManager.Instance.PressPlay();
    }

    public void PressOptions()
    {
        UIManager.Instance.PressOptions();
    }

    public void PressControls()
    {
        UIManager.Instance.PressControls();
    }

    public void PressCredits()
    {
        UIManager.Instance.PressCredits();
    }

    public void PressQuit()
    {
        UIManager.Instance.PressQuit();
    }


}
