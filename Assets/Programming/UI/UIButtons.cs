using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    //gooby ahh script for buttons that arent in the main menu so i can still reference uimanager
    //in stuff that doesnt contain the initial uimanager script its confusing but like dont question it

    public void PressReturnToLevel()
    {
        UIManager.Instance.PressReturnToLevel();
    }

    public void PressMainMenu()
    {
        UIManager.Instance.PressMainMenu();
    }

    public void PressStartGame()
    {
        UIManager.Instance.PressStartGame();
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
