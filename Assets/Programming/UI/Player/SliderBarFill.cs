using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// unknown owner.

// Modified by domibron.

[Obsolete("Going to replace with new system at some point.", false)]
public class SliderBarFill : MonoBehaviour
{
    PlayerMovementHandler playerMovement;
    Health playerHealth;
    PlayerAttackHandler playerAttack;

    private Slider sliderBar;

    public enum SliderType
    {
        Health,
        Stamina,
        ChargedButtonTimeHeld
    }

    [Header("Which slider bar is this for?")]
    public SliderType whichSlider;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = PlayerReferenceFetcher.instance.GetPlayerReference().GetComponent<PlayerMovementHandler>();
        playerHealth = PlayerReferenceFetcher.instance.GetPlayerReference().GetComponent<Health>();
        playerAttack = PlayerReferenceFetcher.instance.GetPlayerReference().GetComponent<PlayerAttackHandler>();

        sliderBar = GetComponent<Slider>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (whichSlider)
        {
            case SliderType.Health:
                sliderBar.value = playerHealth.GetHealthNormalized();
                break;
            case SliderType.Stamina:
                sliderBar.value = playerMovement.GetStaminaNormalized();
                break;
            case SliderType.ChargedButtonTimeHeld:
                //if heavy attack is unlocked then set slider bar value
                if (playerAttack.heavyAttackUnlocked)
                {
                    //if not currently in the heavy attack set value to charge value
                    //this check is to stop the charging whenever the normal heavy is pressed
                    sliderBar.value = playerAttack.GetChargedHeavyAmountNormalized();
                }
                //otherwise slider bar value is always 0
                else
                {
                    sliderBar.value = 0;
                }

                break;
            default:
                break;
        }
    }
}
