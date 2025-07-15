using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|
// © 2025 Dominic McNeill dommcneill@outlook.com
// Licensed for use within the Wraiths of Retail by Smash Keyboard Studios (SKS) only.
// Redistribution or modification outside of this project is prohibited without explicit written permission.
// For full license terms, see DOMIBRON_CODE_LICENSE.md at the project root.


public class AbilityIcon : MonoBehaviour
{
    public Image coolDownOverlayImage;
    public TMP_Text coolDownText;

    public GameObject iconObject;

    public enum AbilityType
    {
        HeavyAttack,
        Shield,
    }

    public AbilityType abilityType;

    private PlayerAttackHandler playerAttackHandler;
    private ShieldAbility shieldAbility;

    void Start()
    {
        GameObject playerRef = PlayerReferenceFetcher.instance.GetPlayerReference();

        playerAttackHandler = playerRef.GetComponent<PlayerAttackHandler>();
        shieldAbility = playerRef.GetComponent<ShieldAbility>();
    }

    // Update is called once per frame
    void Update()
    {
        if (abilityType == AbilityType.HeavyAttack)
        {
            if (!playerAttackHandler.heavyAttackUnlocked)
            {
                iconObject.SetActive(false);

                // coolDownOverlayImage.fillAmount = 1;
                // coolDownText.text = string.Empty;

                return;
            }
            else
            {
                iconObject.SetActive(true);
            }

            if (playerAttackHandler.GetChargedHeavyAmountNormalized() > 0)
            {
                // if we are charging.
                coolDownOverlayImage.fillAmount = 1 - playerAttackHandler.GetChargedHeavyAmountNormalized();
                coolDownText.text = string.Empty;
            }
            else if (playerAttackHandler.IsHeavyAttackOnCoolDown())
            {
                // are we on cool down?
                coolDownOverlayImage.fillAmount = playerAttackHandler.GetHeavyAttackCoolDownNormalized();
                coolDownText.text = (playerAttackHandler.GetHeavyAttackCoolDownNormalized() * playerAttackHandler.heavyAttackCoolDown).ToString("F1");
            }
            else
            {
                // ability must be ready
                coolDownOverlayImage.fillAmount = 0;
                coolDownText.text = string.Empty;
            }
        }
        else if (abilityType == AbilityType.Shield)
        {
            if (!shieldAbility.unlockedShield)
            {
                iconObject.SetActive(false);

                // coolDownOverlayImage.fillAmount = 1;
                // coolDownText.text = string.Empty;

                return;
            }
            else
            {
                iconObject.SetActive(true);
            }



            if (shieldAbility.IsShieldActive() && !shieldAbility.IsShieldOnCoolDown())
            {
                // if we are using the shield.
                coolDownOverlayImage.fillAmount = 1 - shieldAbility.GetDurationNormalized();
                coolDownText.text = (shieldAbility.GetDurationNormalized() * shieldAbility.shieldDurationTime).ToString("F1");
            }
            else if (shieldAbility.IsShieldOnCoolDown())
            {
                // are we on cool down?
                coolDownOverlayImage.fillAmount = Mathf.Clamp(shieldAbility.GetCoolDownNormalized(), 0, 1);
                coolDownText.text = (shieldAbility.GetCoolDownNormalized() * shieldAbility.shieldCoolDownTime).ToString("F1");
            }
            else if (shieldAbility.IsShieldReady())
            {
                // ability must be ready
                coolDownOverlayImage.fillAmount = 0;
                coolDownText.text = string.Empty;
            }

        }
    }
}
