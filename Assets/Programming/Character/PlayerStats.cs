using UnityEngine;

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

    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        inputHandler = PlayerInputHandler.Instance;
    }

    public bool TakeDamage(float Ammount)
    {
        throw new System.NotImplementedException();
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

            if (inputHandler.InteractionTriggered)
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
