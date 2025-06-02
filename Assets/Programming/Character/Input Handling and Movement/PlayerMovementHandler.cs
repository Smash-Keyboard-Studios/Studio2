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



public class PlayerMovementHandler : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float sprintMultiplier = 2f;

    public float maxStamina = 100f;
    public float staminaDecrease = 7f;
    public float staminaIncrease = 10f;
    public float staminaChargeRate = 1.5f;

    public float gravity = 9.81f;

    [Header("no touch. will fuck.")]
    public bool canSprint = true;

    private Transform playerRotationObject;

    private CharacterController cc;

    private Animator playerAnimator;

    private Vector2 inputVector = Vector2.zero;

    private bool isSprinting = false;

    private float currentStamina = 0f;

    private Vector3 playerVelocity = Vector3.zero;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        playerAnimator = GetComponentInChildren<Animator>();
        playerRotationObject = transform.Find("PlayerRotation");

        currentStamina = maxStamina;

        GetComponent<Health>().onDeathEvent += OnDeath;
    }

    private void OnDeath()
    {
        playerAnimator.SetBool("isDead", true);
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMovement();
        HandleGravity();
        cc.Move(playerVelocity * Time.deltaTime);

        HandleAnimations();
    }

    private void HandlePlayerMovement()
    {
        float speed = walkSpeed;

        if (isSprinting && inputVector.magnitude > 0 && currentStamina > 0 && canSprint)
        {
            speed = walkSpeed * sprintMultiplier;
        }

        if (isSprinting && inputVector.magnitude > 0 && canSprint && cc.velocity.magnitude > 0)
        {
            currentStamina -= staminaDecrease * (staminaChargeRate * Time.deltaTime);
        }
        else if (!isSprinting)
        {
            currentStamina += staminaIncrease * (staminaChargeRate * Time.deltaTime);
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        Vector3 playerMoveVector = transform.TransformDirection(new Vector3(inputVector.x, 0, inputVector.y)).normalized * speed;
        playerVelocity.x = playerMoveVector.x;
        playerVelocity.z = playerMoveVector.z;
    }

    private void HandleGravity()
    {
        if (cc.isGrounded)
        {
            playerVelocity.y = -gravity;
        }
        else
        {
            playerVelocity.y -= gravity * Time.deltaTime;
        }
    }

    private void HandleAnimations()
    {
        Vector3 movementWithoutGravity = new Vector3(playerVelocity.x, 0, playerVelocity.z);

        playerAnimator.SetFloat("Horiz", Vector3.Dot(playerRotationObject.right, movementWithoutGravity));
        playerAnimator.SetFloat("Vert", Vector3.Dot(playerRotationObject.forward, movementWithoutGravity));
        //Debug.Log(CurrentMovement);
        playerAnimator.SetFloat("MoveSpeed", movementWithoutGravity.magnitude);
    }

    public void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    public float GetStaminaNormalized()
    {
        return currentStamina / maxStamina;
    }

    public void Warp(Vector3 newPos)
    {
        cc.enabled = false;
        transform.position = newPos;
        cc.enabled = true;
    }
}
