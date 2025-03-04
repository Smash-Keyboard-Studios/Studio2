using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCMainMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.PressMainMenu();
        }
    }
}
