using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCReturns : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.PressReturn();
        }
    }
}
