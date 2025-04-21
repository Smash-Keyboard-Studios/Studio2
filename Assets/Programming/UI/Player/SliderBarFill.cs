using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBarFill : MonoBehaviour
{
    PlayerStats playerStats;
    PlayerAttack playerAttack;

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
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
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
                sliderBar.maxValue = playerAttack.MaxChargedHeavyDmg;
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
                //if heavy attack is unlocked then set slider bar value
                if (playerAttack.unlockedHeavyAttack)
                {
                    //if not currently in the heavy attack set value to charge value
                    //this check is to stop the charging whenever the normal heavy is pressed
                    sliderBar.value = playerAttack.heavyAttacking ? 0 : playerAttack.ChargedHeavyDmg;
                }
                //otherwise slider bar value is always 0
                else
                {
                    sliderBar.value = 0;
                }
                
                break;
            default:
                break;
        }
    }
}
