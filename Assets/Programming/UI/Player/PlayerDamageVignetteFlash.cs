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



public class PlayerDamageVignetteFlash : MonoBehaviour
{
    public CanvasGroup vignetteCanvasGroup;

    [Range(0f, 1f)]
    public float percentageToStartFlashing = 0.2f;

    public float lowHealthFlashRate = 1f;

    [Range(0f, 1f)]
    public float lowHealthMinAlpha = 0.2f;

    public float flashOnHitDuration = 0.3f;
    private float flashTimer = 0f;

    // TODO, flash intensity based on damage taken.

    private Health playerHealth;

    void Start()
    {
        playerHealth = PlayerReferenceFetcher.instance.GetPlayerReference().GetComponent<Health>();

        playerHealth.onTakeDamage += OnTakeDamage;


        vignetteCanvasGroup.alpha = 0;
    }

    void Update()
    {
        if (playerHealth.GetHealthNormalized() < percentageToStartFlashing)
        {
            vignetteCanvasGroup.alpha = Mathf.Lerp(lowHealthMinAlpha, 1f, (Mathf.Sin(Time.time * lowHealthFlashRate) + 1) / 2f);
        }
        else if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;

            vignetteCanvasGroup.alpha = Mathf.Sin(Mathf.PI * (flashTimer / flashOnHitDuration));
        }
        else
        {
            vignetteCanvasGroup.alpha = 0f;
        }
    }

    private void OnTakeDamage(float amount)
    {
        flashTimer = flashOnHitDuration;
    }


}
