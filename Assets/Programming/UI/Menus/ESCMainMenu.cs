using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCMainMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIManager.Instance.PressMainMenu();
        }
    }
}
