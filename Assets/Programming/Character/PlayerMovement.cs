using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;
    [SerializeField] private float staminaDecrease = 5.0f;
    [SerializeField] private float staminaIncrease = 2.5f;
    [SerializeField] private float staminaChargeRate = 1.5f;

    [Header("Gravity Parameters")]
    [SerializeField] private float gravity = 9.81f;

    private CharacterController characterController;
    private PlayerInputHandler inputHandler;
    private PlayerStats stats;
    private Vector3 currentMovement;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputHandler = PlayerInputHandler.Instance;
        stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        HandleMovement();
    }
    
    void HandleMovement()
    {
        float speed = walkSpeed;

        if (stats.PlayerStamina > staminaDecrease)
        {
            speed = walkSpeed * (inputHandler.SprintValue > 0 ? sprintMultiplier : 1f);
        }

        if (inputHandler.SprintValue > 0)
        {
            stats.PlayerStamina = stats.PlayerStamina - staminaDecrease * (staminaChargeRate * Time.deltaTime);
        }
        else if (inputHandler.SprintValue <= 0)
        {
            stats.PlayerStamina = stats.PlayerStamina + staminaIncrease * (staminaChargeRate * Time.deltaTime);
        }

        Vector3 inputDirection = new Vector3(inputHandler.MoveInput.x, 0f, inputHandler.MoveInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        worldDirection.Normalize();

        currentMovement.x = worldDirection.x * speed;
        currentMovement.z = worldDirection.z * speed;

        HandleGravity();
        characterController.Move(currentMovement * Time.deltaTime);
    }

    void HandleGravity()
    {
      currentMovement.y -= gravity * Time.deltaTime;   
    }
}
