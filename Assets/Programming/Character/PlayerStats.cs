using System;
using UnityEngine;

//Script by Aaron Wing

//HOW TO USE: In order for the player to gain any of these stats the Player and PlayerInputHandler prefabs should be in the scene. The scripts themselves should already be attached to the indvivdual Game Object.
//This script is attached to the player.
public class PlayerStats : MonoBehaviour, IDamagable
{
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

    private void Awake()
    {
        InputHandler = PlayerInputHandler.Instance;
    }

    public bool TakeDamage(float Ammount)
    {
        PlayerHealth -= Ammount;
        return true;
    }

    private void Update()
    {
        PlayerStamina = Mathf.Clamp(PlayerStamina, PlayerMinStamina, PlayerMaxStamina);
        PlayerHealth = Mathf.Clamp(PlayerHealth, PlayerMinHealth, PlayerMaxHealth);
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
