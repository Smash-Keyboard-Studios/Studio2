using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoClipPlayerController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private Vector3 currentMovement;

    // Start is called before the first frame update
    void Start()
    {
        inputHandler = PlayerInputHandler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 InputDirection = new Vector3(inputHandler.MoveInput.x, 0f, inputHandler.MoveInput.y);
        Vector3 WorldDirection = transform.TransformDirection(InputDirection);
        WorldDirection.Normalize();

        float speed = 3f;

        if (inputHandler.SprintValue > 0)
        {
            speed = 9f;
        }

        currentMovement.x = WorldDirection.x * speed;
        currentMovement.z = WorldDirection.z * speed;

        transform.Translate(currentMovement * Time.deltaTime);
    }
}
