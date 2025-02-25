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
        Stamina,
        ChargedButtonTimeHeld
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
                sliderBar.maxValue = playerStats.PlayerMaxHealth;
                break;
            case SliderType.Stamina:
                sliderBar.maxValue = playerStats.PlayerMaxStamina;
                break;
            case SliderType.ChargedButtonTimeHeld:
                sliderBar.maxValue = 20; //set this to the max charge value
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
            case SliderType.ChargedButtonTimeHeld:
                sliderBar.value = 20; //set this to the time that the charge button has been held for
                break;
            default:
                break;
        }
    }
}
