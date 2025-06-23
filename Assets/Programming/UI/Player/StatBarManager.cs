using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



public class StatBarManager : MonoBehaviour
{
    public float healthBarSmoothing = 1f;
    public float healthLossBarSmoothing = 1f;

    public float waitBeforeUpdating = 1f;

    private float waitTimer = 0f;

    private float currentValue = 0f;
    private float targetValue = 0f;
    private float animationTimer = 0f;

    public Image healthBarImage;
    public Image healthBarLossImage;
    public Image staminaBarImage;

    private Health health;
    private PlayerMovementHandler movementHandler;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerReferenceFetcher.instance == null) Debug.LogError("Cannot get the player reference!");

        GameObject playerGO = PlayerReferenceFetcher.instance.GetPlayerReference();

        health = playerGO.GetComponent<Health>();
        health.onAddToHealth += AddToHealth;

        movementHandler = playerGO.GetComponent<PlayerMovementHandler>();
    }

    private void AddToHealth(float obj)
    {
        if (obj < 0)
        {
            waitTimer = waitBeforeUpdating;
            animationTimer = 0f;
            if (healthBarLossImage.fillAmount < healthBarImage.fillAmount) healthBarLossImage.fillAmount = healthBarImage.fillAmount;

            currentValue = healthBarLossImage.fillAmount;
            targetValue = health.GetHealthNormalized();
        }

        // if (health.GetHealthNormalized() < currentValue && obj > 0)
        // {
        //     healthBarLossImage.fillAmount = healthBarImage.fillAmount;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTimer <= 0 && health.GetHealthNormalized() <= currentValue)
        {
            animationTimer += healthLossBarSmoothing * Time.deltaTime;
            healthBarLossImage.fillAmount = Mathf.Lerp(currentValue, targetValue, animationTimer);
        }
        else if (waitTimer <= 0 && health.GetHealthNormalized() > currentValue)
        {
            healthBarLossImage.fillAmount = healthBarImage.fillAmount;
        }
        else
        {
            waitTimer -= Time.deltaTime;
        }

        // float displacement = Mathf.Abs(healthBarImage.fillAmount - health.GetHealthNormalized());

        healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, health.GetHealthNormalized(), healthBarSmoothing * Time.deltaTime);
        staminaBarImage.fillAmount = movementHandler.GetStaminaNormalized();
    }
}
