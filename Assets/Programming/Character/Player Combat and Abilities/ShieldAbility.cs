using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldAbility : MonoBehaviour
{
    public bool unlockedShield;

    public bool isShieldInCoolDown;
    public bool isShieldActive;

    public float shieldUsageSec = 3f;
    public float coolDownSec = 5f;

    public GameObject HooverModel;

    private HealthWithBasicShield healthWithBasicShield;

    void Start()
    {
        healthWithBasicShield = GetComponent<HealthWithBasicShield>();

        isShieldActive = false;
        isShieldInCoolDown = false;
    }

    void Update()
    {


        HooverModel.SetActive(unlockedShield);
    }

    public void OnBlock()
    {
        if (unlockedShield)
        {
            if (!isShieldInCoolDown)
            {
                healthWithBasicShield.ActivateShield();
                isShieldActive = true;
                StartCoroutine(ShieldUsage());
            }
        }
    }

    IEnumerator ShieldUsage()
    {
        yield return new WaitForSeconds(shieldUsageSec);

        healthWithBasicShield.BreakShield();

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
