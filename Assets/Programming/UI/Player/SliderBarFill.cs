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
        KillCount
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
            case SliderType.KillCount:
                sliderBar.maxValue = 20; //this is a goal for number of enemies to kill, might want to change this value depending on which level
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
            case SliderType.KillCount:
                sliderBar.value = 20; //change this to number of enemies the player has killed in scene
                break;
            default:
                break;
        }
    }
}
