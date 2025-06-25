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
// © 2025 Dominic McNeill dommcneill@outlook.com


public class BossHealthBar : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private GameObject shieldOverlay;

    [SerializeField]
    private CanvasGroup Mask;

    [SerializeField]
    private Health health;

    [SerializeField]
    private float fadeRate = 1f;

    private bool overlayVisible = false;

    private float fadeTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(overlayVisible);

        healthBar.fillAmount = health.GetHealthNormalized();

        print(health.GetType().Name);

        if (health.GetType() == typeof(HealthWithBasicShield))
            shieldOverlay.SetActive(((HealthWithBasicShield)health).shieldActive);
        else
            shieldOverlay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (overlayVisible && fadeTimer < 1) fadeTimer += Time.deltaTime * fadeRate;
        else if (!overlayVisible && fadeTimer > 0) fadeTimer -= Time.deltaTime * fadeRate;

        Mask.alpha = fadeTimer;

        canvas.SetActive(overlayVisible);


        healthBar.fillAmount = health.GetHealthNormalized();

        if (health.GetType() == typeof(HealthWithBasicShield))
            shieldOverlay.SetActive(((HealthWithBasicShield)health).shieldActive);
    }

    public void ShowHealthBar()
    {
        overlayVisible = true;
    }

    public void HideHealthBar()
    {
        overlayVisible = false;
    }
}
