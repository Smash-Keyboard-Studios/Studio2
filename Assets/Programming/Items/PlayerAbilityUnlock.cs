using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


public enum AbilityType
{
    heavyAttack,
    shieldAbility,
}

public class PlayerAbilityUnlock : MonoBehaviour
{
    public AbilityType abilityType;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Health playerHealth = other.GetComponent<Health>();

        if (playerHealth == null) return;

        if (playerHealth.GetHealthNormalized() <= 0) return;

        switch (abilityType)
        {
            case AbilityType.heavyAttack:
                other.GetComponent<PlayerAttackHandler>().heavyAttackUnlocked = true;
                break;

            case AbilityType.shieldAbility:
                other.GetComponent<ShieldAbility>().unlockedShield = true;
                break;
        }

        //StartCoroutine(ShowAbilityUnlocked("Heavy Attack"));
        //StartCoroutine(ShowAbilityUnlocked("Shield"));

        Destroy(gameObject);
    }
}
