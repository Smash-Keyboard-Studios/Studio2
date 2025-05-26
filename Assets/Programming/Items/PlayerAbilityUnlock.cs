using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        PlayerStats pStats = other.GetComponent<PlayerStats>();

        if (pStats == null) return;

        if (pStats.isDead) return;

        switch (abilityType)
        {
            case AbilityType.heavyAttack:
                other.GetComponent<PlayerAttack>().unlockedHeavyAttack = true;
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
