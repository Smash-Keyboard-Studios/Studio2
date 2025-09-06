using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//to be added to level 2 button, will enable/disable the button based on whether we have reached level 2
public class Level2SelectDisable : MonoBehaviour
{
    [SerializeField] private Button level2Button;

    private void OnEnable()
    {
        level2Button = GetComponent<Button>();

        //enabled/diable button based on value in playerprefs
        if (PlayerPrefs.GetFloat("Level2Unlocked") > 0)
        {
            level2Button.interactable = true;
        }
        else
        {
            level2Button.interactable = false;
        }
    }
}
