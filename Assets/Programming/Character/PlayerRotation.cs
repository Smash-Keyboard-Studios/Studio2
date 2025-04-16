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
        //get centre point of player in screen space
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position) - Vector3.up;
        //get direction of player to mouse point
        Vector3 playerToMouse = Input.mousePosition - playerScreenPoint;

        // we swap the y to z as up on screen is forward so we want that to be z.
        Vector3 lookDirectionInWorld = new Vector3(playerToMouse.x, transform.position.y, playerToMouse.y);

        // we take the player's position and add the normalized direction vector. we just want a point around the player
        transform.LookAt(transform.position + lookDirectionInWorld.normalized);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); //prevents rotation around other axis
    }
}
