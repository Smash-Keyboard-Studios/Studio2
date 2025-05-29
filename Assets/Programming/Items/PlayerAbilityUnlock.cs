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
