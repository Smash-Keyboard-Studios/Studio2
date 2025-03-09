using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCGameMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIManager.Instance.PressGameMenu();
        }
    }
}
