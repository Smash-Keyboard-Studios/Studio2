using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



public class NoClipPlayerController : MonoBehaviour
{
    private Vector2 inputVector;
    private bool isSprinting;

    private Vector3 currentMovement;

    // Start is called before the first frame update
    void Start()
    {
        // inputHandler = PlayerInputHandler.Instance;
    }

    public void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 InputDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        Vector3 WorldDirection = transform.TransformDirection(InputDirection);
        WorldDirection.Normalize();

        float speed = 3f;

        if (isSprinting)
        {
            speed = 9f;
        }

        currentMovement.x = WorldDirection.x * speed;
        currentMovement.z = WorldDirection.z * speed;

        transform.Translate(currentMovement * Time.deltaTime);
    }
}
