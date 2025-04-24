using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

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

    public bool isDead;

    [Header("Interaction")]
    public GameObject InteractionUI;
    public GameObject AbilityUnlockedUI;

    [Header("Unlockable Abilities")]
    [SerializeField] private GameObject HooverModel;

    private PlayerInputHandler InputHandler;

    protected Animator MyAnim;

    [HideInInspector] public AudioSource audioSource;

    [Header("Other GameObject References")]
    public GameObject MainCharacter;
    public GameObject playerRotation;
    public GameObject DeathScreen;

    private PlayerAttack attackScript;
    private ShieldAbility shieldScript;
    private DamageIndicator indicator;

    private void Start()
    {
        //Gets the InputHandler from the PlayerInputHandler instance
        InputHandler = PlayerInputHandler.Instance;

        //animator
        MyAnim = MainCharacter.GetComponent<Animator>();

        //audio
        audioSource = GetComponent<AudioSource>();

        //script references
        attackScript = GetComponent<PlayerAttack>();
        shieldScript = GetComponent<ShieldAbility>();
        indicator = GetComponent<DamageIndicator>();

        //interaction ui starts as disabled
        InteractionUI.SetActive(false);

        //player models start as disabled if that ability is locked (enabled on unlock)
        HooverModel.SetActive(shieldScript.unlockedShield);
    }

    public bool TakeDamage(float Amount)
    {
        if (!shieldScript.isShieldActive)
        {
            PlayerHealth -= Amount;
            AudioManager.Instance.PlayAudio(false, false, audioSource, "Plr_GetHit");
            indicator.FlashStart();
            return true;
        }
        else if (shieldScript.isShieldActive)
        {
            //if shield has blocked damage play the shield deflect sound
            AudioManager.Instance.PlayAudio(false, false, audioSource, "Plr_ShieldHit");
        }
        return false;
    }

    private void Update()
    {
        PlayerStamina = Mathf.Clamp(PlayerStamina, PlayerMinStamina, PlayerMaxStamina); //Clamps the PlayerStamina between the min and max stamina values
        PlayerHealth = Mathf.Clamp(PlayerHealth, PlayerMinHealth, PlayerMaxHealth); //Clamps the PlayerHealth between the min and max health values

        //When the player loses all of their Health they will die.
        if (PlayerHealth == 0)
        {
            if (!isDead)
            {
                isDead = true;

                playerRotation.GetComponent<PlayerRotation>().enabled = false;
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<PlayerAttack>().enabled = false;

                MyAnim.SetTrigger("Dead");
                MyAnim.SetBool("isDead", true);
                DeathScreen.SetActive(true);
            }
        }
        else
        {
            isDead = false;

            playerRotation.GetComponent<PlayerRotation>().enabled = true;
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<PlayerAttack>().enabled = true;

            MyAnim.SetBool("isDead", false);
            DeathScreen.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //interactable objects

        switch (other.gameObject.tag)
        {
            case "UnlockableHammer":
                InteractionUI.SetActive(true);

                if (InputHandler.InteractionTriggered)
                {
                    //enable heavy attack
                    attackScript.unlockedHeavyAttack = true;

                    Destroy(other.gameObject);
                    InteractionUI.SetActive(false);

                    StartCoroutine("ShowAbilityUnlocked", "Heavy Attack");
                }

                break;

            case "UnlockableShield":
                InteractionUI.SetActive(true);

                if (InputHandler.InteractionTriggered)
                {
                    //enable Shield
                    shieldScript.unlockedShield = true;

                    HooverModel.SetActive(true);

                    Destroy(other.gameObject);
                    InteractionUI.SetActive(false);

                    StartCoroutine("ShowAbilityUnlocked", "Shield");
                }

                break;

            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //interactable objects

        if(other.gameObject.CompareTag("UnlockableHammer") ||
            other.gameObject.CompareTag("UnlockableShield"))
        {
            InteractionUI.SetActive(false);
        }
    }

    private IEnumerator ShowAbilityUnlocked(string whichAbility)
    {
        AbilityUnlockedUI.SetActive(true);
        AbilityUnlockedUI.GetComponentInChildren<TextMeshProUGUI>().text = whichAbility + " Unlocked!";

        yield return new WaitForSeconds(2);

        AbilityUnlockedUI.SetActive(false);
    }
}
