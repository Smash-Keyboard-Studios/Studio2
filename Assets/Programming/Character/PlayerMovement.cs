using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script by Aaron Wing

//HOW TO USE: In order for the player to move make sure you have both the Player and PlayerInputHandler prefabs in the scene. The scripts should already be attached to the individual game objects.
//This script should be attached to the Player.
public class PlayerMovement : MonoBehaviour
{
    //The different movement speeds for the player
    [Header("Movement Speeds")]
    [SerializeField] private float WalkSpeed = 3.0f;
    [SerializeField] private float SprintMultiplier = 2.0f;
    [SerializeField] private float StaminaDecrease = 5.0f;
    [SerializeField] private float StaminaIncrease = 2.5f;
    [SerializeField] private float StaminaChargeRate = 1.5f;

    //Gravity value
    [Header("Gravity Parameters")]
    [SerializeField] private float Gravity = 9.81f;

    //Any extra things
    private CharacterController CharacterController;
    private PlayerInputHandler InputHandler;
    private PlayerStats Stats;
    private Vector3 CurrentMovement;
    private bool CanSprint;

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>(); //Gets the CharacterController from the component
        Stats = GetComponent<PlayerStats>(); //Gets the Stats from the component
    }

    private void Start()
    {
        InputHandler = PlayerInputHandler.Instance; //Gets the InputHandler from the PlayerInputHandler instance
        CanSprint = false; //Sets the bool to false when the game starts
    }

    private void Update()
    {
        HandleMovement(); //Updates the HandleMovement by every frame
    }

    //This handles anything movement related
    void HandleMovement()
    {
        float Speed = WalkSpeed; //The speed the player moves

        //Player can only sprint while they have stamina
        if (Stats.PlayerStamina > StaminaDecrease)
        {
            Speed = WalkSpeed * (InputHandler.SprintValue > 0 ? SprintMultiplier : 1f); //Walkspeed gets multiplied by 2 when the sprint button is pressed otherwise it stays at its original value.
        }

        //Sets CanSprint to true only if the player is moving either in the X or Y direction (Forwards/Back or Left/Right)
        if (InputHandler.MoveInput.x > 0 || InputHandler.MoveInput.y > 0)
        {
            CanSprint = true; //Sets the bool to true
        }

        //If player is pressing the sprint button and CanSprint is true then the player can sprint and it drains 
        if (InputHandler.SprintValue > 0 && CanSprint)
        {
            Stats.PlayerStamina = Stats.PlayerStamina - StaminaDecrease * (StaminaChargeRate * Time.deltaTime); //Player stamina is decreased by the StaminaDecrease value over the course of StaminaChargeRate times by Time.deltaTime
        }
        else if (InputHandler.SprintValue <= 0) //When the player is not sprinting and if the sprint value is bigger or equal to 0 then the sprint value will recharge.
        {
            Stats.PlayerStamina = Stats.PlayerStamina + StaminaIncrease * (StaminaChargeRate * Time.deltaTime); //Player stamina is increased by the StaminaIncrease value over the course of StaminaChargeRate times by Time.deltaTime
            CanSprint = false; //Sets the bool to false
        }

        Vector3 InputDirection = new Vector3(InputHandler.MoveInput.x, 0f, InputHandler.MoveInput.y);
        Vector3 WorldDirection = transform.TransformDirection(InputDirection);
        WorldDirection.Normalize();

        CurrentMovement.x = WorldDirection.x * Speed;
        CurrentMovement.z = WorldDirection.z * Speed; 

        HandleGravity();
        CharacterController.Move(CurrentMovement * Time.deltaTime);
    }

    //This handles the gavity so the player is not floating everywhere.
    void HandleGravity()
    {
        CurrentMovement.y -= Gravity * Time.deltaTime; 
    }
}
