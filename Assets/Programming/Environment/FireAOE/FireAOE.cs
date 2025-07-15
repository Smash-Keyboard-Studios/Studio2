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



public class FireAOE : MonoBehaviour
{
    public float fireTickDamage = 5f;

    public float fireDuration = 3f;

    // we locally store the player refs so we dont do expensive calls multiple times.
    private FireTickManager fireTickManager;
    private IShieldObject playerShield;

    void Start()
    {
        // we try and get the reference.
        if (PlayerReferenceFetcher.instance == null) Debug.LogError($"Cannot get the player reference because the {nameof(PlayerReferenceFetcher)} was not found!");

        GameObject playerGO = PlayerReferenceFetcher.instance.GetPlayerReference();


        fireTickManager = playerGO.GetComponent<FireTickManager>();
        playerShield = playerGO.GetComponent<IShieldObject>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PlayerTag))
        {
            // if the failed to get a reference, try to get it again. // TODO: Error handling for both triggers.
            if (fireTickManager == null) PlayerReferenceFetcher.instance.GetPlayerReference()?.GetComponent<FireTickManager>();
            if (playerShield == null) PlayerReferenceFetcher.instance.GetPlayerReference()?.GetComponent<IShieldObject>();

            if (!playerShield.isShieldActive)
            {
                fireTickManager.SetOnFire(fireDuration, fireTickDamage);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PlayerTag))
        {
            //if (fireTickManager == null) PlayerReferenceFetcher.instance.GetPlayerReference().GetComponent<FireTickManager>();

            if (!playerShield.isShieldActive) fireTickManager.SetOnFire(fireDuration, fireTickDamage);
        }
    }
}
