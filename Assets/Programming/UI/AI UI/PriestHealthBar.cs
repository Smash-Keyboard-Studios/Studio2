using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriestHealthBar : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private Image healthBar;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (overlayVisible && fadeTimer < 1) fadeTimer += Time.deltaTime * fadeRate;
        else if (!overlayVisible && fadeTimer > 0) fadeTimer -= Time.deltaTime * fadeRate;

        Mask.alpha = fadeTimer;

        canvas.SetActive(overlayVisible);


        healthBar.fillAmount = health.GetHealthNormalized();
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
