using System;
using System.Collections;
using TMPro;
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
    public float PlayerMaxStamina = 100;
    public float PlayerHealAmount = 25;

    public bool isDead;

    [Header("Interaction")]
    public GameObject AbilityUnlockedUI; // TODO relocate to its own system.




    private PlayerInputHandler InputHandler;

    protected Animator MyAnim; // ! protected? mmm, typo?

    [HideInInspector] public AudioSource audioSource;

    [Header("Other GameObject References")]
    public GameObject MainCharacter;
    public GameObject playerRotation;

    public event Action onDeathEvent;

    private PlayerAttack attackScript;
    private ShieldAbility shieldScript;
    private DamageIndicator indicator;

    private FloatingTextSystem floatingTextSystem;

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

        floatingTextSystem = GetComponent<FloatingTextSystem>();
    }

    public bool TakeDamage(float Amount)
    {
        if (!shieldScript.isShieldActive)
        {
            PlayerHealth -= Amount;
            AudioManager.Instance.PlayAudio(false, false, audioSource, "Plr_GetHit");
            if (indicator.enabled) indicator.FlashStart(); // ! domibron ~ I added a check because this breaks the AI somehow.
            floatingTextSystem.SpawnText(Amount.ToString("F0"), new Color(0.8f, 0.5f, 0f),
            (6 + (3 * Mathf.Sqrt(Amount))) * UnityEngine.Random.Range(0.4f, 0.4f));
            return true;
        }
        else if (shieldScript.isShieldActive)
        {
            //if shield has blocked damage play the shield deflect sound
            AudioManager.Instance.PlayAudio(false, false, audioSource, "Plr_ShieldHit"); // ! domibron ~ add shield hit VFX too. 
            if (indicator.enabled) indicator.FlashStart(); // ! domibron ~ I added a check because this breaks the AI somehow.
            floatingTextSystem.SpawnText("Blocked", Color.cyan,
            (6 + (3 * Mathf.Sqrt(Amount))) * UnityEngine.Random.Range(0.4f, 0.4f));
        }
        return false;
    }

    private void Update()
    {

        PlayerStamina = Mathf.Clamp(PlayerStamina, 0, PlayerMaxStamina); //Clamps the PlayerStamina between the min and max stamina values
        PlayerHealth = Mathf.Clamp(PlayerHealth, 0, PlayerMaxHealth); //Clamps the PlayerHealth between the min and max health values

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
                onDeathEvent?.Invoke();
            }
        }
        else
        {
            isDead = false; // ! domibron ~ this will allow the player to revive themselves. What is the point of this else? could be removed.

            playerRotation.GetComponent<PlayerRotation>().enabled = true;
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<PlayerAttack>().enabled = true;

            MyAnim.SetBool("isDead", false);
        }
    }

    // TODO relocate on a UI popup manager than on player stats. planning to remove player stats and uncouple player scripts.
    private IEnumerator ShowAbilityUnlocked(string whichAbility)
    {
        AbilityUnlockedUI.SetActive(true);
        AbilityUnlockedUI.GetComponentInChildren<TextMeshProUGUI>().text = whichAbility + " Unlocked!";

        yield return new WaitForSeconds(2);

        AbilityUnlockedUI.SetActive(false);
    }
}
