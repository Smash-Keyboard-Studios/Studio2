using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldownUI : MonoBehaviour
{
    //ui stuff
    private Slider cooldownBar;
    private TextMeshProUGUI cooldownText;

    private bool abilityInUse;
    private bool inCooldown;

    private enum AbilityType
    {
        LightAttack,
        HeavyAttack,
        ChargedHeavyAttack,
        Shield
    }

    [SerializeField] private AbilityType abilityType;

    private GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        //set cooldown bar and text
        cooldownBar = GetComponent<Slider>();
        cooldownText = GetComponentInChildren<TextMeshProUGUI>();

        //set cooldown bar max value and disable it for now
        cooldownBar.enabled = false;

        //set text to nothing for now
        cooldownText.text = string.Empty;

        //set initial values for vars
        abilityInUse = false;
        inCooldown = false;

        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerObject != null) //if weve found player object then set ability in use to the correct ability type
        {
            PlayerAttack playerAttackScript = playerObject.GetComponent<PlayerAttack>();
            ShieldAbility playerShieldScript = playerObject.GetComponent<ShieldAbility>();

            switch (abilityType)
            {
                case AbilityType.LightAttack:
                    abilityInUse = playerAttackScript.lightAttacking;
                    cooldownBar.maxValue = playerAttackScript.LightAttackDelay;
                    break;

                case AbilityType.HeavyAttack:
                    if (playerAttackScript.unlockedHeavyAttack) //if unlocked, set cooldown to whether heavy attacking
                    {
                        abilityInUse = playerAttackScript.heavyAttacking;
                        cooldownBar.maxValue = playerAttackScript.HeavyAttackDelay;
                    }
                    else //otherwise set slider to max so it looks disabled
                    {
                        abilityInUse = false;
                        cooldownBar.enabled = true;
                        cooldownBar.maxValue = 1;
                        cooldownBar.value = cooldownBar.maxValue;
                    }
                    break;

                case AbilityType.Shield:
                    if (playerShieldScript.unlockedShield) //if unlocked, set cooldown to whether using shield
                    {
                        abilityInUse = playerShieldScript.isShieldActive || playerShieldScript.isShieldInCoolDown;
                        cooldownBar.maxValue = playerShieldScript.shieldUsageSec + playerShieldScript.coolDownSec;
                    }
                    else //otherwise set slider to max so it looks disabled
                    {
                        abilityInUse = false;
                        cooldownBar.enabled = true;
                        cooldownBar.maxValue = 1;
                        cooldownBar.value = cooldownBar.maxValue;
                    }
                    break;
            }
        }
        else //otherwise try to find playerobject
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        if (abilityInUse && !inCooldown) { StartCoroutine("TriggerCooldown"); } //trgger cooldown is ability is in use
    }

    private IEnumerator TriggerCooldown()
    {
        inCooldown = true;

        float currentTime = cooldownBar.maxValue; //timer for the cooldown
        float timeIncrements = 0.1f; //how many seconds between current time being updated

        //setting cooldown bar to active and initial values
        cooldownBar.enabled = true;
        cooldownBar.value = currentTime;
        cooldownText.text = currentTime.ToString();

        //loop for cooldown happening
        while (currentTime > 0)
        {
            currentTime -= timeIncrements; //update current time

            //update UI
            cooldownBar.value = currentTime;
            cooldownText.text = currentTime.ToString(); //set text to string of current time
            //cut text to first few chars if longer than 3 chars
            if (cooldownText.text.Length > 3) { cooldownText.text = cooldownText.text.Substring(0, 3); }

            yield return new WaitForSeconds(timeIncrements); //delay
        }

        //cooldown UI is no longer active/visible
        cooldownText.text = string.Empty;
        cooldownBar.enabled = false;

        abilityInUse = false; //no longer using ability
        inCooldown = false;
    }
}
