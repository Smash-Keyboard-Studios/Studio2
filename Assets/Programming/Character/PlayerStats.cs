using System;
using UnityEngine;

//Script by Aaron Wing

//HOW TO USE: In order for the player to gain any of these stats the Player and PlayerInputHandler prefabs should be in the scene. The scripts themselves should already be attached to the indvivdual Game Object.
//This script is attached to the player.
public class PlayerStats : MonoBehaviour, IDamageable
{
    //Player stats value such as current stamina/health and max/min stamina/health
    [Header("Player Stats")]
    public float PlayerHealth = 100;
    public float PlayerStamina = 100;
    public float PlayerMaxHealth = 100;
    public float PlayerMinHealth = 0;
    public float PlayerMaxStamina = 100;
    public float PlayerMinStamina = 0;
    public float PlayerHealAmount = 25;

    [Header("Interaction")]
    public GameObject InteractionUI;

    private PlayerInputHandler InputHandler;

    private void Start()
    {
        InputHandler = PlayerInputHandler.Instance; //Gets the InputHandler from the PlayerInputHandler instance
    }

    public bool TakeDamage(float Ammount)
    {
        PlayerHealth -= Ammount;
        return true;
    }

    private void Update()
    {
        PlayerStamina = Mathf.Clamp(PlayerStamina, PlayerMinStamina, PlayerMaxStamina); //Clamps the PlayerStamina between the min and max stamina values
        PlayerHealth = Mathf.Clamp(PlayerHealth, PlayerMinHealth, PlayerMaxHealth); //Clamps the PlayerHealth between the min and max health values
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("HealthObject"))
        {
            InteractionUI.SetActive(true);

            if (InputHandler.InteractionTriggered)
            {
                if (PlayerHealth < PlayerMaxHealth)
                {
                    PlayerHealth += PlayerHealAmount;
                    Destroy(other.gameObject);
                    InteractionUI.SetActive(false);
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HealthObject"))
        {
            InteractionUI.SetActive(false);
        }
    }
}
