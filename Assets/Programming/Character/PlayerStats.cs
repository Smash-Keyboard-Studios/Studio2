using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamagable
{
    [Header("Player Stats")]
    public float PlayerHealth = 100;
    public float PlayerStamina = 100;

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
        PlayerStamina = Mathf.Clamp(PlayerStamina, 0, 100);
        PlayerHealth = Mathf.Clamp(PlayerHealth, 0, 100);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("HealthObject"))
        {
            InteractionUI.SetActive(true);

            if (inputHandler.InteractionTriggered)
            {
                if (PlayerHealth < 100)
                {
                    PlayerHealth += 25f;
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
