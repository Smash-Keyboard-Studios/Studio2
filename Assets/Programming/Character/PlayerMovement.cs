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
    [SerializeField] private float Gravity = 9.81f * 2;

    //Any extra things
    private CharacterController CharacterController;
    private PlayerInputHandler InputHandler;
    private PlayerStats Stats;
    private Vector3 CurrentMovement;
    private bool CanSprint;
    private bool IsSprinting;

    private Animator MyAnim;
    public GameObject MainCharacter;
    public Transform rotation;

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>(); //Gets the CharacterController from the component
        Stats = GetComponent<PlayerStats>(); //Gets the Stats from the component
    }

    private void Start()
    {
        InputHandler = PlayerInputHandler.Instance; //Gets the InputHandler from the PlayerInputHandler instance
        MyAnim = MainCharacter.GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement(); //Updates the HandleMovement by every frame
        CharacterAnimations(); //This is for animating the character
    }

    //This handles anything movement related
    void HandleMovement()
    {
        float Speed = WalkSpeed; //The speed the player moves

        //Player can only sprint while they have stamina
        if (Stats.PlayerStamina > 0)
        {
            Speed = WalkSpeed * (InputHandler.SprintValue > 0 ? SprintMultiplier : 1f); //Walkspeed gets multiplied by 2 when the sprint button is pressed otherwise it stays at its original value.
        }

        if (Mathf.Abs(InputHandler.MoveInput.x) > 0.1f && InputHandler.SprintValue > 0 || Mathf.Abs(InputHandler.MoveInput.y) > 0.1f && InputHandler.SprintValue > 0)
        {
            IsSprinting = true; //Sets the bool to true
        }
        else
        {
            IsSprinting = false;
        }

        //If player is pressing the sprint button and CanSprint is true then the player can sprint and it drains 
        if (InputHandler.SprintValue > 0 && IsSprinting)
        {
            Stats.PlayerStamina = Stats.PlayerStamina - StaminaDecrease * (StaminaChargeRate * Time.deltaTime); //Player stamina is decreased by the StaminaDecrease value over the course of StaminaChargeRate times by Time.deltaTime
        }
        else if (!IsSprinting) //When the player is not sprinting and if the sprint value is bigger or equal to 0 then the sprint value will recharge.
        {
            Stats.PlayerStamina = Stats.PlayerStamina + StaminaIncrease * (StaminaChargeRate * Time.deltaTime); //Player stamina is increased by the StaminaIncrease value over the course of StaminaChargeRate times by Time.deltaTime
        }

        MyAnim.SetBool("Running", IsSprinting);

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
        if (CharacterController.isGrounded)
        {
            CurrentMovement.y = -Gravity * Time.deltaTime;
        }
        else
        {
            CurrentMovement.y -= Gravity * Time.deltaTime;
        }
    }

    void CharacterAnimations()
    {
        Vector3 playerForward = rotation.forward;
        Vector3 playerRight = rotation.right;

        if (CurrentMovement.magnitude > 0)
        {
            float dot = Vector3.Dot(playerForward, CurrentMovement);

            if (dot > 0.75)
            {

            }

        }
        Vector3 movementWithoutGravity = new Vector3(CurrentMovement.x, 0, CurrentMovement.z);

        MyAnim.SetFloat("Horiz", Vector3.Dot(playerRight, movementWithoutGravity));
        MyAnim.SetFloat("Vert", Vector3.Dot(playerForward, movementWithoutGravity));
        //Debug.Log(CurrentMovement);
        MyAnim.SetFloat("MoveSpeed", Mathf.Max(Mathf.Abs(CurrentMovement.z), Mathf.Abs(CurrentMovement.x)));
    }

    public void Warp(Vector3 newPos)
    {
        CharacterController.enabled = false;
        transform.position = newPos;
        CharacterController.enabled = true;
    }
}