using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCReturnsToLevel : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.PressReturnToLevel();
        }
    }
}
