using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBarFill : MonoBehaviour
{
    PlayerStats playerStats;

    private Slider sliderBar;

    public enum SliderType
    {
        Health, 
        Stamina
    }

    [Header("Which slider bar is this for?")]
    public SliderType whichSlider;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        sliderBar = GetComponent<Slider>();

        switch (whichSlider)
        {
            case SliderType.Health:
                sliderBar.maxValue = 100; //change this to playerStats.maxHealth
                break;
            case SliderType.Stamina:
                sliderBar.maxValue = 100; //change this to playerStats.maxStamina
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (whichSlider)
        {
            case SliderType.Health:
                sliderBar.value = playerStats.PlayerHealth;
                break;
            case SliderType.Stamina:
                sliderBar.value = playerStats.PlayerStamina;
                break;
            default:
                break;
        }
    }
}
