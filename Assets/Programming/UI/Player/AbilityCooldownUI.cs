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

    public bool abilityInUse;
    [SerializeField] private float cooldownTime;

    private bool inCooldown;

    // Start is called before the first frame update
    void Start()
    {
        //set cooldown bar and text
        cooldownBar = GetComponent<Slider>();
        cooldownText = GetComponentInChildren<TextMeshProUGUI>();

        //set cooldown bar max value and disable it for now
        cooldownBar.maxValue = cooldownTime;
        cooldownBar.enabled = false;
        //set text to nothing for now
        cooldownText.text = string.Empty;

        //set initial values for vars
        abilityInUse = false;
        inCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (abilityInUse && !inCooldown) { StartCoroutine("TriggerCooldown"); }
    }

    private IEnumerator TriggerCooldown()
    {
        inCooldown = true;

        float currentTime = cooldownTime; //timer for the cooldown
        float timeIncrements = 0.1f; //how many seconds between current time being updated

        //setting cooldown bar to active and initial values
        cooldownBar.enabled = true;
        cooldownBar.value = currentTime;
        cooldownText.text = currentTime.ToString();

        //loop for cooldown happening
        while (currentTime > 0.01)
        {
            currentTime -= timeIncrements; //update current time

            //update UI
            cooldownBar.value = currentTime;
            cooldownText.text = currentTime.ToString().Substring(0, 3); //set text to first few chars of current time

            yield return new WaitForSeconds(timeIncrements); //delay
        }

        //cooldown UI is no longer active/visible
        cooldownText.text = string.Empty;
        cooldownBar.enabled = false;

        abilityInUse = false; //no longer using ability
        inCooldown = false;
    }
}
