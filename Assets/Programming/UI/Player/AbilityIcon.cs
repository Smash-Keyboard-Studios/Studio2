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
    [Header("Heavy Icon")]
    public Image heavyCoolDownOverlayImage;
    public TMP_Text heavyCoolDownText;

    public GameObject heavyIconObject;

    [Header("Shield Icon")]
    public Image shieldCoolDownOverlayImage;
    public TMP_Text shieldCoolDownText;

    public GameObject shieldIconObject;


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
        HandleHeavyIcon();
        HandleShieldIcon();
    }

    private void HandleShieldIcon()
    {
        if (!shieldAbility.unlockedShield)
        {
            shieldIconObject.SetActive(false);

            // coolDownOverlayImage.fillAmount = 1;
            // coolDownText.text = string.Empty;

            return;
        }
        else
        {
            shieldIconObject.SetActive(true);
        }



        if (shieldAbility.IsShieldActive() && !shieldAbility.IsShieldOnCoolDown())
        {
            // if we are using the shield.
            shieldCoolDownOverlayImage.fillAmount = 1 - shieldAbility.GetDurationNormalized();
            shieldCoolDownText.text = (shieldAbility.GetDurationNormalized() * shieldAbility.shieldDurationTime).ToString("F1");
        }
        else if (shieldAbility.IsShieldOnCoolDown())
        {
            // are we on cool down?
            shieldCoolDownOverlayImage.fillAmount = Mathf.Clamp(shieldAbility.GetCoolDownNormalized(), 0, 1);
            shieldCoolDownText.text = (shieldAbility.GetCoolDownNormalized() * shieldAbility.shieldCoolDownTime).ToString("F1");
        }
        else if (shieldAbility.IsShieldReady())
        {
            // ability must be ready
            shieldCoolDownOverlayImage.fillAmount = 0;
            shieldCoolDownText.text = string.Empty;
        }
    }

    private void HandleHeavyIcon()
    {
        if (!playerAttackHandler.heavyAttackUnlocked)
        {
            heavyIconObject.SetActive(false);

            // coolDownOverlayImage.fillAmount = 1;
            // coolDownText.text = string.Empty;

            return;
        }
        else
        {
            heavyIconObject.SetActive(true);
        }

        if (playerAttackHandler.GetChargedHeavyAmountNormalized() > 0)
        {
            // if we are charging.
            heavyCoolDownOverlayImage.fillAmount = 1 - playerAttackHandler.GetChargedHeavyAmountNormalized();
            heavyCoolDownText.text = string.Empty;
        }
        else if (playerAttackHandler.IsHeavyAttackOnCoolDown())
        {
            // are we on cool down?
            heavyCoolDownOverlayImage.fillAmount = playerAttackHandler.GetHeavyAttackCoolDownNormalized();
            heavyCoolDownText.text = (playerAttackHandler.GetHeavyAttackCoolDownNormalized() * playerAttackHandler.heavyAttackCoolDown).ToString("F1");
        }
        else
        {
            // ability must be ready
            heavyCoolDownOverlayImage.fillAmount = 0;
            heavyCoolDownText.text = string.Empty;
        }
    }
}
