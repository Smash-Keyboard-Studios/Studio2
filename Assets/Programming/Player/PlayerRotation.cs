using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|


public class PlayerRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;

    private Quaternion currentRotation;

    void Start()
    {
        GetComponentInParent<Health>().onDeath += OnDeath;
    }

    private void OnDeath()
    {
        enabled = false;
    }

    void Update()
    {
        if (!UIManager.Instance.inGameMenu && !UIManager.Instance.inDialogueMenu)
        {
            HandleRotationInput();
        }
    }

    void HandleRotationInput() //Rotate player to face mouse.
    {
        //get centre point of player in screen space. // TODO camera can be cached to improve some performance but will only do so little.
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position - Vector3.up);
        //get direction of player to mouse point
        Vector3 playerToMouse = Input.mousePosition - playerScreenPoint;

        // we swap the y to z as up on screen is forward so we want that to be z.
        Vector3 lookDirectionInWorld = new Vector3(playerToMouse.x, transform.position.y, playerToMouse.y);

        // we take the player's position and add the normalized direction vector. we just want a point around the player
        Vector3 targetDirection = lookDirectionInWorld.normalized;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);


        currentRotation = Quaternion.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = currentRotation;

        //transform.LookAt(transform.position + lookDirectionInWorld.normalized); // TODO add some lerp here. 
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); //prevents rotation around other axis
    }
}
