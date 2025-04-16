using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAbility : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    public GameObject activeShield;

    public bool isShieldInCoolDown;
    public bool isShieldActive;
    public bool unlockedShield;

    public float shieldUsageSec = 3f;
    public float coolDownSec = 5f;

    void Start()
    {
        inputHandler = PlayerInputHandler.Instance;

        activeShield.SetActive(false);
        isShieldActive = false;
        isShieldInCoolDown = false;
    }

    void Update()
    {
        if (unlockedShield)
        {
            if (!isShieldInCoolDown)
            {
                if (inputHandler.BlockTriggered)
                {
                    activeShield.SetActive(true);
                    isShieldActive = true;
                    StartCoroutine(ShieldUsage());
                }
            }
        }
    }

    IEnumerator ShieldUsage()
    {
        yield return new WaitForSeconds(shieldUsageSec);
        activeShield.SetActive(false);
        isShieldActive = false;
        isShieldInCoolDown = true;
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolDownSec);
        isShieldInCoolDown = false;
    }
}
