using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCGameMenu : MonoBehaviour
{
    float pauseCooldown;

    private void Update()
    {
        pauseCooldown -= Time.deltaTime;
        if (pauseCooldown < 0 && Input.GetKeyUp(KeyCode.Escape))
        {
            UIManager.Instance.PressGameMenu();
            pauseCooldown = 0.05f;
        }
    }
}
