using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    void Update()
    {
        if(!UIManager.Instance.inGameMenu && !UIManager.Instance.inDialogueMenu)
        {
            HandleRotationInput();
        }
    }

    void HandleRotationInput() //Rotate player to face mouse.
    {
        Vector3 MouseScreenToCameraSpace = new Vector3(Input.mousePosition.x, 0f, Input.mousePosition.y);
        Vector3 PlayerScreenToCameraSpace = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, 0f, Camera.main.WorldToScreenPoint(transform.position).y);
        Vector3 PlayerToMouse = MouseScreenToCameraSpace - PlayerScreenToCameraSpace;
        transform.LookAt(PlayerToMouse);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
