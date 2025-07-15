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

public class HealthObject : MonoBehaviour
{
    public float healAmount = 30f;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Health playerHealth = other.GetComponent<Health>();

        if (playerHealth == null) return;

        if (playerHealth.GetHealthNormalized() <= 0) return;

        if (playerHealth.GetHealthNormalized() < 1f)
        {
            // Debug.Log("Healed player");

            // AudioManager.Instance.PlayAudio(false, false, other.GetComponent<AudioSource>(), "Plr_Heal");

            playerHealth.AddToHealth(healAmount);

            Destroy(gameObject);
        }


    }
}
