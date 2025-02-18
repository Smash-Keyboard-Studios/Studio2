using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script by Aaron Wing

//HOW TO USE: In order for the player to move make sure you have both the Player and PlayerInputHandler prefabs in the scene. The scripts should already be attached to the individual game objects.
//This script should be attached to the Player.
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float WalkSpeed = 3.0f;
    [SerializeField] private float SprintMultiplier = 2.0f;
    [SerializeField] private float StaminaDecrease = 5.0f;
    [SerializeField] private float StaminaIncrease = 2.5f;
    [SerializeField] private float StaminaChargeRate = 1.5f;

    [Header("Gravity Parameters")]
    [SerializeField] private float Gravity = 9.81f;

    private CharacterController CharacterController;
    private PlayerInputHandler InputHandler;
    private PlayerStats Stats;
    private Vector3 CurrentMovement;

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        Stats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        InputHandler = PlayerInputHandler.Instance;
    }

    private void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float Speed = WalkSpeed;

        if (Stats.PlayerStamina > StaminaDecrease)
        {
            Speed = WalkSpeed * (InputHandler.SprintValue > 0 ? SprintMultiplier : 1f);
        }

        if (InputHandler.SprintValue > 0)
        {
            Stats.PlayerStamina = Stats.PlayerStamina - StaminaDecrease * (StaminaChargeRate * Time.deltaTime);
        }
        else if (InputHandler.SprintValue <= 0)
        {
            Stats.PlayerStamina = Stats.PlayerStamina + StaminaIncrease * (StaminaChargeRate * Time.deltaTime);
        }

        Vector3 InputDirection = new Vector3(InputHandler.MoveInput.x, 0f, InputHandler.MoveInput.y);
        Vector3 WorldDirection = transform.TransformDirection(InputDirection);
        WorldDirection.Normalize();

        CurrentMovement.x = WorldDirection.x * Speed;
        CurrentMovement.z = WorldDirection.z * Speed;

        HandleGravity();
        CharacterController.Move(CurrentMovement * Time.deltaTime);
    }

    void HandleGravity()
    {
        CurrentMovement.y -= Gravity * Time.deltaTime;
    }
}
