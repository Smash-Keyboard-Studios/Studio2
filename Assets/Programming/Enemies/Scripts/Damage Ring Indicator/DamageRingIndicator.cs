using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRingIndicator : MonoBehaviour
{
    public GameObject damageRingGameObject;

    private float ringGrowTime = 0;

    private float currentRingGrowTime = 0;

    private float ringDiameter = 1;


    private float shrinkTime = 1f;

    private float currentShrinkTime = 1f;


    private bool showRing = false;

    void Update()
    {
        if (currentRingGrowTime < ringGrowTime)
            currentRingGrowTime += Time.deltaTime;

        if (currentShrinkTime > 0)
            currentShrinkTime -= Time.deltaTime * 2f;

        damageRingGameObject.SetActive(showRing || (currentShrinkTime / shrinkTime) > 0);

        // stop divide by zero error.
        if (showRing)
        {
            damageRingGameObject.transform.localScale = Vector3.one * ringDiameter * EaseOutBack(currentRingGrowTime / ringGrowTime);
        }
        else if ((currentShrinkTime / shrinkTime) > 0)
        {
            damageRingGameObject.transform.localScale = Vector3.one * ringDiameter * EaseInOutCubic(currentShrinkTime / shrinkTime);
        }


    }

    // TODO these should be in a static function on a tween class
    float EaseOutBack(float x)
    {

        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }

    float EaseInOutCubic(float x)
    {
        return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
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
        if (showRing)
            currentShrinkTime = shrinkTime;

        showRing = false;
        //ringDiameter = 1f;
    }
}
