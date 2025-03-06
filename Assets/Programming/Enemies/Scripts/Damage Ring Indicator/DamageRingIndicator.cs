using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRingIndicator : MonoBehaviour
{
    public GameObject damageRingGameObject;

    private float ringGrowTime = 0;

    private float currentRingGrowTime = 0;

    private float ringDiameter = 1;

    private bool showRing = false;

    void Update()
    {
        if (currentRingGrowTime < ringGrowTime)
            currentRingGrowTime += Time.deltaTime;

        damageRingGameObject.SetActive(showRing);

        // stop divide by zero error.
        if (!showRing) return;

        damageRingGameObject.transform.localScale = Vector3.one * ringDiameter * EaseOutBack(currentRingGrowTime / ringGrowTime);



    }

    float EaseOutBack(float x)
    {

        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }


    public void ShowRing(float chargeTime, float radius)
    {
        ringGrowTime = chargeTime;
        currentRingGrowTime = 0;

        ringDiameter = radius * 2f;

        showRing = true;
    }

    public void HideRing()
    {
        showRing = false;
        ringDiameter = 1f;
    }
}
