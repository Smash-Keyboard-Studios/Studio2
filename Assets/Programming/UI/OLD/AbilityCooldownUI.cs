using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Owner Soph

[Obsolete("This is a outdated version!", true)]
public class AbilityCooldownUI : MonoBehaviour
{
    //ui stuff
    private Slider coolDownBar;
    private TextMeshProUGUI coolDownText;

    private bool abilityInUse;
    private bool inCoolDown;

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
        coolDownBar = GetComponent<Slider>();
        coolDownText = GetComponentInChildren<TextMeshProUGUI>();

        //set cool down bar max value and disable it for now
        coolDownBar.enabled = false;

        //set text to nothing for now
        coolDownText.text = string.Empty;

        //set initial values for vars
        abilityInUse = false;
        inCoolDown = false;

        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

    }

    // holy :/
    private IEnumerator TriggerCooldown()
    {
        inCoolDown = true;

        float currentTime = coolDownBar.maxValue; //timer for the cooldown
        float timeIncrements = 0.1f; //how many seconds between current time being updated

        //setting cooldown bar to active and initial values
        coolDownBar.enabled = true;
        coolDownBar.value = currentTime;
        coolDownText.text = currentTime.ToString();

        //loop for cooldown happening
        while (currentTime > 0)
        {
            currentTime -= timeIncrements; //update current time

            //update UI
            coolDownBar.value = currentTime;
            coolDownText.text = currentTime.ToString(); //set text to string of current time
            //cut text to first few chars if longer than 3 chars
            if (coolDownText.text.Length > 3) { coolDownText.text = coolDownText.text.Substring(0, 3); }

            yield return new WaitForSeconds(timeIncrements); //delay
        }

        //cooldown UI is no longer active/visible
        coolDownText.text = string.Empty;
        coolDownBar.enabled = false;

        abilityInUse = false; //no longer using ability
        inCoolDown = false;
    }
}
