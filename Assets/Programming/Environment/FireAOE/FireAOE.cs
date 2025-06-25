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



public class FireAOE : MonoBehaviour
{
    public float fireTickDamage = 5f;

    public float fireDuration = 3f;

    private FireTickManager fireTickManager;
    private HealthWithBasicShield playerHealthWithBasicShield;

    void Start()
    {
        if (PlayerReferenceFetcher.instance == null) Debug.LogError($"Cannot get the player reference because the {nameof(PlayerReferenceFetcher)} was not found!");

        GameObject playerGO = PlayerReferenceFetcher.instance.GetPlayerReference();


        fireTickManager = playerGO.GetComponent<FireTickManager>();
        playerHealthWithBasicShield = playerGO.GetComponent<HealthWithBasicShield>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //if (fireTickManager == null) PlayerReferenceFetcher.instance.GetPlayerReference()?.GetComponent<FireTickManager>();

            if (!playerHealthWithBasicShield.isActiveAndEnabled) fireTickManager.SetOnFire(fireDuration, fireTickDamage);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //if (fireTickManager == null) PlayerReferenceFetcher.instance.GetPlayerReference().GetComponent<FireTickManager>();

            if (!playerHealthWithBasicShield.isActiveAndEnabled) fireTickManager.SetOnFire(fireDuration, fireTickDamage);
        }
    }
}
